using System.Data.SqlClient;
using System.Data;
using System;

namespace Library_Management_System.DAO
{
    public class HelperDAO
    {
        public static object NullAsDbNull(object valor)
        {
            if (valor == null)
                return DBNull.Value;
            else
                return valor;
        }

        public static void ExecutaProc(string nomeProc,
                                      SqlParameter[] parametros) {
            using (SqlConnection conexao = ConexaoBD.GetConexao()) {
                using (SqlCommand comando = new SqlCommand(nomeProc, conexao)) {
                        comando.CommandType = CommandType.StoredProcedure;
                        if (parametros != null)
                            comando.Parameters.AddRange(parametros);
                        comando.ExecuteNonQuery();
                }
            }
        }

        public static DataTable ExecutaProcSelect(string nomeProc, SqlParameter[] parametros) {
            using (SqlConnection conexao = ConexaoBD.GetConexao()) {
                using (SqlDataAdapter adapter = new SqlDataAdapter(nomeProc, conexao)) {
                    if (parametros != null)
                        adapter.SelectCommand.Parameters.AddRange(parametros);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    return tabela;
                }
            }
        }
    }
}
