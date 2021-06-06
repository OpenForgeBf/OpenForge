// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NLog;

namespace OpenForge.Server.Database
{
    public abstract class DBObject<T> where T : DBObject<T>
    {
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        private static readonly object s_initLock = new object();
        private static List<T> s_db = new List<T>();
        private static bool s_isInitialized = false;

        public static string DatabaseLocation => $"Database/{typeof(T).Name}/";
        public string ObjectLocation => $"{DatabaseLocation}/{ObjectID}.json";

        public virtual bool MemoryOnly => false;

        public string ObjectID { get; set; } = Guid.NewGuid().ToString();
        public long LastSave { get; set; } = 0;


        public virtual void Update()
        {
            EnsureInitialization();
            LastSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            lock (s_db)
            {
                if (!s_db.Contains(this))
                {
                    s_db.Add((T)this);
                }
            }
            if (!MemoryOnly)
            {
                File.WriteAllText(ObjectLocation, JsonConvert.SerializeObject(this));
            }

            Logger.Trace(() => $"Database [{typeof(T).Name}] saved object {ObjectID}");
        }
        public virtual void Delete()
        {
            EnsureInitialization();
            lock (s_db)
            {
                s_db.Remove((T)this);
            }

            Logger.Trace(() => $"Database [{typeof(T).Name}] deleted object {ObjectID}");
        }
        public void ReplaceWith(T obj)
        {
            Logger.Trace(() => $"Database [{typeof(T).Name}] replacing object {ObjectID}");
            EnsureInitialization();
            obj.ObjectID = ObjectID;
            Delete();
            obj.Update();
        }

        public static void LoadDatabase()
        {
            var dir = new DirectoryInfo(DatabaseLocation);
            if (!dir.Exists)
            {
                Directory.CreateDirectory(DatabaseLocation);
            }

            var objs = new List<T>();
            var count = 0;
            foreach (var file in dir.GetFiles())
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(file.FullName));
                    objs.Add(obj);
                    count++;
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, $"Failed to initialize instance of type [{typeof(T).Name}].");
                }
            }
            s_db = objs;
            Logger.Info($"Initialized Database [{typeof(T).Name}] with {count} objects");
        }

        public static void Access(Action<List<T>> act)
        {
            EnsureInitialization();
            lock (s_db)
            {
                act(s_db);
            }
        }
        public static R Access<R>(Func<List<T>, R> act)
        {
            EnsureInitialization();
            lock (s_db)
            {
                return act(s_db);
            }
        }
        public static T FirstOrDefault(Func<T, bool> where)
        {
            EnsureInitialization();
            lock (s_db)
            {
                return s_db.FirstOrDefault(where);
            }
        }
        public static int Count()
        {
            EnsureInitialization();
            return s_db.Count;
        }
        public static List<R> Select<R>(Func<T, R> select)
        {
            EnsureInitialization();
            lock (s_db)
            {
                return s_db.Select(select).ToList();
            }
        }
        public static List<T> Where(Func<T, bool> where)
        {
            EnsureInitialization();
            lock (s_db)
            {
                return s_db.Where(where).ToList();
            }
        }


        private static void EnsureInitialization()
        {
            lock (s_initLock)
            {
                if (!s_isInitialized)
                {
                    LoadDatabase();
                    s_isInitialized = true;
                }
            }
        }
    }
}
