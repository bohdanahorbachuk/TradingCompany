using DAL.Interfaces;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.ADO
{
    public class PersonDAL : IPersonDAL
    {
        private string _connStr;

        public PersonDAL(string connStr)
        {
            this._connStr = connStr;
        }

        public PersonDTO CreatPerson(PersonDTO person)
        {
            using (SqlConnection conn = new SqlConnection(this._connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                conn.Open();
                comm.CommandText = "INSERT into Person (ID_Role,FirstName, LastName,Login,Password) output INSERTED.ID_Person values (@idRole,@firstName,@lastName,@log,@passw)";

                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@idRole", person.RoleID);
                comm.Parameters.AddWithValue("@firstName", person.FirstName);
                comm.Parameters.AddWithValue("@lastName", person.LastName);
                comm.Parameters.AddWithValue("@log", person.Login);
                comm.Parameters.AddWithValue("@passw", person.Password);


                person.ID_Person = (int)comm.ExecuteScalar();
                conn.Close();

                return person;

            }
        }

        public void DeletePerson(int personId)
        {
            using (SqlConnection conn = new SqlConnection(this._connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                comm.CommandText = "DELETE from Person WHERE ID_Person = @id";
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@id", personId);
                conn.Open();
                comm.ExecuteNonQuery();
            }
        }

        public List<PersonDTO> GetAllPerson()
        {
            using (SqlConnection conn = new SqlConnection(this._connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                comm.CommandText = "select * from Person";
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                var person = new List<PersonDTO>();
                while (reader.Read())
                {
                    person.Add(new PersonDTO
                    {
                        ID_Person = (int)reader["ID_Person"],
                        RoleID = (int)reader["RoleID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Login = reader["Login"].ToString(),
                        Password = reader["Password"].ToString(),
                        RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString()),
                        RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString())
                    });
                }
                conn.Close();
                return person;
            }
        }

        public PersonDTO GetPersonById(int personId)
        {
            using (SqlConnection conn = new SqlConnection(this._connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                comm.CommandText = $"select * from Person where ID_Person = {personId}";
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();

                PersonDTO myPerson = new PersonDTO();
                while (reader.Read())
                {
                    myPerson = new PersonDTO
                    {
                        ID_Person = (int)reader["ID_Person"],
                    };
                }
                return myPerson;
            }
        }

        public PersonDTO UpdatePerson(PersonDTO p, int id)
        {
            using (SqlConnection conn = new SqlConnection(this._connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                comm.CommandText = $"UPDATE Person set ID_Role = @id_rol, FirstName = @firsn, LastName = @lastn, Login = @log ,Password = @passw, RowUpdateTime = @rowup WHERE ID_Person = {id} "; ;

                comm.Parameters.Clear();

                comm.Parameters.AddWithValue("@id_rol", p.RoleID);
                comm.Parameters.AddWithValue("@firsn", p.FirstName);
                comm.Parameters.AddWithValue("@lastn", p.LastName);
                comm.Parameters.AddWithValue("@log", p.Login);
                comm.Parameters.AddWithValue("@passw", p.Password);
                comm.Parameters.AddWithValue("@rowup", p.RowUpdateTime);



                conn.Open();

                p.ID_Person = comm.ExecuteNonQuery();
                conn.Close();
                return p;
            }
        }
    }
}