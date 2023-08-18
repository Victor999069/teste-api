using Npgsql;
using System.Data;

namespace APIAplication
{
    public class FuncoesDaApi
    {
        public static object IIf(bool expression, object truePart, object falsePart)
        { return expression ? truePart : falsePart; }

        public static string QuoteStr(string cConteudo)
        { return "'" + cConteudo + "'"; }

        public static DataSet oxcQuery(string cSql, Conexao ds_conexao)
        {
            DataSet dsDados = new DataSet();

            NpgsqlDataAdapter QrDados = new NpgsqlDataAdapter(cSql, ds_conexao.DsCps);
            QrDados.Fill(dsDados);

            return dsDados;
        }
    }
}
