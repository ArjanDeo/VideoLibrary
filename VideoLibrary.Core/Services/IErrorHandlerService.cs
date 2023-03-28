using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace VideoLibrary.Core.Services
{
    public interface IErrorHandlerService
    {
        void SendToGER(IExceptionHandlerPathFeature exceptionDetails, Guid UserId, int? errorCode = null);
        void SendToGERMessage(string Message, Guid UserId, int? errorCode = null);
    }
    public class ErrorHandlerService : IErrorHandlerService
    {
        private readonly IConfiguration _configuration;
        public ErrorHandlerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendToGER(IExceptionHandlerPathFeature exceptionDetails, Guid UserId, int? errorCode = null)
        {
            var exPath = exceptionDetails.Path;
            var exMessage = exceptionDetails.Error.Message;
            var exStackTrace = exceptionDetails.Error.StackTrace;

            using (SqlConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_CreateGlobalErrorHandling", dbConnection);
                dbConnection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParam1 = cmd.Parameters.AddWithValue("@application", "PurePractice");
                SqlParameter sqlParam2 = cmd.Parameters.AddWithValue("@errorPath", exPath);
                SqlParameter sqlParam3 = cmd.Parameters.AddWithValue("@errorMessage", exMessage);
                SqlParameter sqlParam4 = cmd.Parameters.AddWithValue("@errorStack", exStackTrace);
                SqlParameter sqlParam5 = cmd.Parameters.AddWithValue("@errorCode", errorCode.GetValueOrDefault());
                SqlParameter sqlParam6 = cmd.Parameters.AddWithValue("@userId", UserId);
                sqlParam1.SqlDbType = SqlDbType.NChar;
                sqlParam2.SqlDbType = SqlDbType.NChar;
                sqlParam3.SqlDbType = SqlDbType.NChar;
                sqlParam4.SqlDbType = SqlDbType.NVarChar;
                sqlParam5.SqlDbType = SqlDbType.Int;
                sqlParam6.SqlDbType = SqlDbType.UniqueIdentifier;
                cmd.ExecuteNonQuery();
                dbConnection.Close();
            }
        }

        public void SendToGERMessage(string Message, Guid UserId, int? errorCode = null)
        {


            using (SqlConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_CreateGlobalErrorHandling", dbConnection);
                dbConnection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParam1 = cmd.Parameters.AddWithValue("@application", "PurePractice");
                SqlParameter sqlParam2 = cmd.Parameters.AddWithValue("@errorPath", "VideoUpload");
                SqlParameter sqlParam3 = cmd.Parameters.AddWithValue("@errorMessage", Message);
                SqlParameter sqlParam4 = cmd.Parameters.AddWithValue("@errorStack", "na");
                SqlParameter sqlParam5 = cmd.Parameters.AddWithValue("@errorCode", errorCode.GetValueOrDefault());
                SqlParameter sqlParam6 = cmd.Parameters.AddWithValue("@userId", UserId);
                sqlParam1.SqlDbType = SqlDbType.NChar;
                sqlParam2.SqlDbType = SqlDbType.NChar;
                sqlParam3.SqlDbType = SqlDbType.NChar;
                sqlParam4.SqlDbType = SqlDbType.NVarChar;
                sqlParam5.SqlDbType = SqlDbType.Int;
                sqlParam6.SqlDbType = SqlDbType.UniqueIdentifier;
                cmd.ExecuteNonQuery();
                dbConnection.Close();
            }
        }

    }
}
