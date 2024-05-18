using System.Data.SqlClient;

namespace Library_Management_System.DAO
{
    public class ConexaoBD
    {
        public static SqlConnection GetConexao()
        {
            string strCon = "Data Source=146.235.34.235; Database=LMS; user id=sa; password=RogerServer3301";
            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
