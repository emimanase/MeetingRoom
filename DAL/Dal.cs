using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Logger;

namespace DAL
{
    public class Dal : IDisposable
    {
        private SqlConnection _dbConnection;
        private string _connectionString;
        public Dal()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DAL.Properties.Settings.MeetingRoomDBConnectionString"].ConnectionString;
            _dbConnection = new SqlConnection(_connectionString);
            _dbConnection.Open();
        }

        public DataTable GetAllEmployees()
        {
            try
            {
                return RunSPReturnDt("dbo.GetAllEmployees");
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                return new DataTable();
            }
        }

        public DataTable GetEmployeeByCnp(string cnp)
        {
            try
            {
                return RunSPReturnDt("dbo.GetEmployeeByCNP", new DalParam("@CNP", SqlDbType.VarChar, 0, cnp));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                return new DataTable();
            }
        }

        public DataTable GetAllMeetingRooms()
        {
            try
            {
                return RunSPReturnDt("dbo.GetAllMeetingRooms");
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                return new DataTable();
            }
            
        }

        public DataTable GetMeetingRoomById(int id)
        {
            try
            {
                return RunSPReturnDt("dbo.GetMeetingRoomById", new DalParam("@Id", SqlDbType.Int, 0, id));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                return new DataTable();
            }
        }

        public DataTable GetTimetableForMeetingRoom(int roomId)
        {
            try
            {
                return RunSPReturnDt("dbo.GetTimetableForMeetingRoom", new DalParam("@RoomId", SqlDbType.Int, 0, roomId));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                return new DataTable();
            }
        }

        public DataTable GetTimetableHistoryForMeetingRoom(int roomId)
        {
            try
            {
                return RunSPReturnDt("dbo.GetTimetableHistoryForMeetingRoom",
                    new DalParam("@RoomId", SqlDbType.Int, 0, roomId));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
        }

        public void InsertEntryInTimetable(DateTime from, DateTime to, int roomId, string cnp)
        {
            try
            {
                RunSp("dbo.InsertTimetableEntry", 
                new DalParam("@From", SqlDbType.DateTime, 0, from),
                new DalParam("@To", SqlDbType.DateTime, 0, to),
                new DalParam("@RoomId", SqlDbType.Int, 0, roomId),
                new DalParam("@CNP", SqlDbType.VarChar, 0, cnp));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
        }

        public void UpdateEntryInTimetable(int id, DateTime from, DateTime to, int roomId, string cnp)
        {
            try
            {
                RunSp("dbo.UpdateTimetableEntry", new DalParam("@Id", SqlDbType.Int, 0, id),
                new DalParam("@From", SqlDbType.DateTime, 0, from),
                new DalParam("@To", SqlDbType.DateTime, 0, to),
                new DalParam("@RoomId", SqlDbType.Int, 0, roomId),
                new DalParam("@CNP", SqlDbType.VarChar, 0, cnp));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
        }

        public void RemoveEntryInTimetable(int id)
        {
            try
            {
                RunSp("dbo.RemoveTimetableEntry", new DalParam("@Id", SqlDbType.Int, 0, id));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }

        }

        public void InsertEmployee(string cnp, string firstName, string lastName, string role, string rooms)
        {
            try
            {
                RunSp("dbo.InsertEmployee", new DalParam("@CNP", SqlDbType.VarChar, 0, cnp),
                new DalParam("@FirstName", SqlDbType.VarChar, 0, firstName),
                new DalParam("@LastName", SqlDbType.VarChar, 0, lastName),
                new DalParam("@Role", SqlDbType.VarChar, 0, role),
                new DalParam("@Rooms", SqlDbType.VarChar, 0, rooms));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
        }

        public void InsertRoom(int id, int capacity, string assets)
        {
            try
            {
                RunSp("dbo.InsertRoom", new DalParam("@Id", SqlDbType.Int, 0, id),
                new DalParam("@Capacity", SqlDbType.Int, 0, capacity),
                new DalParam("@Assets", SqlDbType.VarChar, 0, assets));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
        }

        public void RemoveEmployee(string cnp)
        {
            try
            {
                RunSp("dbo.RemoveEmployee", new DalParam("@CNP", SqlDbType.VarChar, 0, cnp));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
        }

        public void RemoveRoom(int id)
        {
            try
            {
                RunSp("dbo.RemoveRoom", new DalParam("@Id", SqlDbType.Int, 0, id));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
        }

        public void UpdateRoom(int id, int capacity, string assets)
        {
            try
            {
                RunSp("dbo.UpdateRoom", new DalParam("@Id", SqlDbType.Int, 0, id),
                    new DalParam("@Capacity", SqlDbType.Int, 0, capacity),
                    new DalParam("@Assets", SqlDbType.VarChar, 0, assets));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
        }

        public void UpdateEmployee(string cnp, string firstName, string lastName, string role, string rooms)
        {
            try
            {
                RunSp("dbo.UpdateEmployee", new DalParam("@CNP", SqlDbType.VarChar, 0, cnp),
                new DalParam("@FirstName", SqlDbType.VarChar, 0, firstName),
                new DalParam("@LastName", SqlDbType.VarChar, 0, lastName),
                new DalParam("@Role", SqlDbType.VarChar, 0, role),
                new DalParam("@Rooms", SqlDbType.VarChar, 0, rooms));
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
        }

        private void RunSp(string spName, params DalParam[] sqlParams)
        {
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                command.Connection = _dbConnection;
                foreach (var sqlParam in sqlParams)
                {
                    command.Parameters.Add(new SqlParameter(sqlParam.Name, sqlParam.ParamType, sqlParam.Size)
                    {
                        Value = sqlParam.Value
                    });
                }

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
            finally
            {
                command.Dispose();
            }
            
        }

        private DataTable RunSPReturnDt(string spName, params DalParam[] sqlParams)
        {
            DataTable dataTable = new DataTable();

            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = spName;
            command.Connection = _dbConnection;
            foreach (var sqlParam in sqlParams)
            {
                command.Parameters.Add(new SqlParameter(sqlParam.Name, sqlParam.ParamType, sqlParam.Size)
                {
                    Value = sqlParam.Value
                });
            }

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            
            try
            {
                sqlDataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                EventLogger.Error(ex);
                throw;
            }
            finally
            {
                command.Dispose();
                sqlDataAdapter.Dispose();
            }

            return dataTable;

        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbConnection != null && _dbConnection.State != ConnectionState.Executing)
                {
                    _dbConnection.Close();
                    _dbConnection.Dispose();
                }
            }
        }  
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}
