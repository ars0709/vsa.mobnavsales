using System;
using System.Collections.Generic;
using System.IO;
using vsa.mobnavsales.library.BussinessLogic;

namespace vsa.mobnavsales.library.DataAccessLayer
{
    public class TaskRepository
    {
        DL.TaskDatabase db = null;
        protected static string dbLocation;
        protected static TaskRepository me;

        static TaskRepository()
        {
            me = new TaskRepository();
        }

        protected TaskRepository()
        {
            // set the db location
            dbLocation = DatabaseFilePath;

            // instantiate the database	
            db = new Tasky.DL.TaskDatabase(dbLocation);
        }

        public static string DatabaseFilePath
        {
            get
            {
                var sqliteFilename = "TaskDB.db3";
#if NETFX_CORE
                var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
#else

#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
	            var path = sqliteFilename;
#else

#if __ANDROID__
                // Just use whatever directory SpecialFolder.Personal returns
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "../Library/"); // Library folder
#endif
                var path = Path.Combine(libraryPath, sqliteFilename);
#endif

#endif
                return path;
            }
        }

        
    }
}