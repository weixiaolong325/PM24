using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PM24
{
    public class SQLHelper
    {
        public static string connStr = ConfigurationManager.ConnectionStrings["pm24"].ToString();

        #region SqlDataReader
        /// <summary>
        /// 返回SqlDataReader
        /// </summary>
        /// <param name="sql">sql或储存过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlDataReader sdr = null;
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = cmdType;
                sdr = cmd.ExecuteReader();
            }
            return sdr;
        }
        #endregion
        #region SqlDataReader,带参数
        /// <summary>
        /// 返回SqlDataReader,带参数
        /// </summary>
        /// <param name="sql">sql或储存过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="param">参数(数组)</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlDataReader sdr = null;
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = cmdType;
                cmd.Parameters.AddRange(param);
                sdr = cmd.ExecuteReader();
            }
            return sdr;
        }
        #endregion

        #region ExecuteNonQuery 增删改或执行储存过程
        /// <summary>
        /// 增删改或执行储存过程
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="cmdType">命令类型</param>
        ///  <returns>受影响行数</returns>
        public static int ExecuteNonQuery(string sql, CommandType cmdType)
        {
            int row = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdType;
                    row = cmd.ExecuteNonQuery();
                }
            }
            return row;
        }
        #endregion
        #region ExecuteNonQuery 增删改或执行储存过程,带参数
        /// <summary>
        /// 增删改或执行储存过程,带参数
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="param">参数</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteNonQuery(string sql, CommandType cmdType, SqlParameter[] param)
        {
            int row = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(param);
                    row = cmd.ExecuteNonQuery();
                }
            }
            return row;
        }
        #endregion

        #region 查询 返回DataTable
        /// <summary>
        /// 执行sql返回DataTable
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="cmdType">参数类型</param>
        /// <returns>DataTable</returns>
        public static DataTable Dt(string sql, CommandType cmdType)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdType;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }
        #endregion
        #region 查询 返回DataTable,带参数
        /// <summary>
        /// 执行sql返回DataTable,带参数
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="param">参数</param>
        /// <returns>DataTable</returns>
        public static DataTable Dt(string sql, CommandType cmdType, SqlParameter[] param)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(param);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }
        #endregion

        #region 查询 返回DataSet
        /// <summary>
        /// 执行sql返回DataSet
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="cmdType">参数类型</param>
        /// <returns>DataSet</returns>
        public static DataSet Ds(string sql, CommandType cmdType)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdType;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }
            return ds;
        }
        #endregion
        #region 查询 返回DataSet，带参数
        /// <summary>
        /// 执行sql返回DataSet,带参数
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="param">参数</param>
        /// <returns>DataSet</returns>
        public static DataSet Ds(string sql, CommandType cmdType, SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(param);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }
            return ds;
        }
        #endregion

        #region ExecuteScalar 返回结果集第一行第一列
        /// <summary>
        /// 返回结果集第一行第一列
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="cmdType">命令类型</param>
        /// <returns>第一行第一列</returns>
        public static string ExecuteScalar(string sql, CommandType cmdType)
        {
            string result = null;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdType;
                    result = cmd.ExecuteScalar().ToString();
                }
            }
            return result;
        }
        #endregion
        #region ExecuteScalar 返回结果集第一行第一列，带参数
        /// <summary>
        /// 返回结果集第一行第一列 带参数
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="param">参数</param>
        /// <returns>第一行第一列</returns>
        public static string ExecuteScalar(string sql, CommandType cmdType, SqlParameter[] param)
        {
            string result = null;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(param);
                    result = cmd.ExecuteScalar().ToString();
                }
            }
            return result;
        }
        #endregion
    }
}