using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;

namespace WikitekMotorCycleMechanik.LocalDatabase
{
    public static class TodoItemDatabase
    {
        static SQLiteAsyncConnection Database;

        #region OEM LOCAL DATA
        public static async Task<SQLiteAsyncConnection> CreateOemResultTable()
        {
            try
            {
                Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                CreateTableResult result = await Database.CreateTableAsync<OemResult>();
                return Database;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<SQLiteAsyncConnection> CreateOemTable()
        {
            try
            {
                Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                CreateTableResult result = await Database.CreateTableAsync<OemResult>();
                return Database;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<SQLiteAsyncConnection> CreateSegmentTable()
        {
            try
            {
                Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                CreateTableResult result = await Database.CreateTableAsync<OemResult>();
                return Database;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<int> SetOemResultData(OemResult model)
        {
            try
            {
                await Database.InsertAsync(model);
                return 1;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"SET LOCAL DATA ERROR : {ex.Message}, LINE : {765}");
                return 100;
            }
        }
        #endregion
    }
}
