using Npgsql;

namespace APIAplication
{
    public class Conexao
    {
        private static string strConexao = "Server=189.14.33.13;port=5455;User Id=postgres;Password=descloc;Database=tsmc;Timeout=0;CommandTimeout=0";

        private NpgsqlConnection conn;
        public Conexao()
        {
            this.conn = new NpgsqlConnection(strConexao);


        }

        public NpgsqlConnection DsCps
        {
            get { return this.conn; }
            set { this.conn = value; }
        }
        public void Conecta()
        {

            conn.Open();



            NpgsqlCommand cmdsql = new NpgsqlCommand("SET application_name = 'ExcErp-Api'", conn);
            cmdsql.CommandTimeout = 300;
            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();



        }

        public void StartTransaction()
        {
            NpgsqlCommand cmdsql = new NpgsqlCommand("START TRANSACTION", conn);

            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();
        }

        public void Commit()
        {
            NpgsqlCommand cmdsql = new NpgsqlCommand("COMMIT", conn);

            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();
        }

        public void RollBack()
        {
            NpgsqlCommand cmdsql = new NpgsqlCommand("rollback", conn);

            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();
        }

        public void ExecutarSql(string cComandoSql)
        {
            using (NpgsqlCommand cmdsql = new NpgsqlCommand(cComandoSql, conn))
            {

                //cmdsql.CommandTimeout = timeout;
                cmdsql.ExecuteNonQuery();

            }

        }

        public void ExecutarSql(NpgsqlCommand cmdsql)
        {

            cmdsql.ExecuteNonQuery();
            cmdsql.Dispose();
        }

        public string CriarTemporaria(string cSql, string cCamposAdicionais = "", string cName = "", bool lTemporario = true)
        {
            string cNameTmp = "";
            string cSqlCreate = "";

            Random randNum = new Random();

            System.DateTime DataHoje = DateTime.Now;

            if (cName != string.Empty)
                cNameTmp = cName;
            else
                cNameTmp = "tmp" + DataHoje.ToLongTimeString().Replace(":", "") + DataHoje.Millisecond.ToString() + randNum.Next().ToString();


            cSqlCreate = "CREATE <TEMP> TABLE IF NOT EXISTS  " + cNameTmp;

            if (lTemporario)
                cSqlCreate = cSqlCreate.Replace("<TEMP>", "TEMPORARY");
            else
                cSqlCreate = cSqlCreate.Replace("<TEMP>", "");

            if (cSql == string.Empty)
            {
                cSqlCreate += "( tmp_id serial primary key " + FuncoesDaApi.IIf(cCamposAdicionais != string.Empty, "," + cCamposAdicionais, "") + ");";
            }
            else
            {
                cSqlCreate += " as " + cSql + ";";

                cSqlCreate += " ALTER TABLE " + cNameTmp + " ADD COLUMN tmp_id SERIAL PRIMARY KEY;";

                if (cCamposAdicionais != string.Empty)
                {
                    cSqlCreate += " ALTER TABLE " + cNameTmp + " ADD COLUMN " + cCamposAdicionais + ";";
                }

            }


            try
            {
                ExecutarSql(cSqlCreate);
            }
            catch (Exception ex)
            {

                cNameTmp = "ERROR";

            }


            return cNameTmp;
        }
        public void desconecta()
        {

            conn.Close();

        }
    }
}
