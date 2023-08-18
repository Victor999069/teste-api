using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace APIAplication.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ClienteController : ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast")]
        public string Get()
        {
            Conexao ds_conexao = new Conexao();

            ds_conexao.Conecta();

            DataSet QrCadastro = FuncoesDaApi.oxcQuery("select cad_id,cad_razao_social from cad_cadastro limit 1", ds_conexao);

            string cNome = QrCadastro.Tables[0].Rows[0]["cad_razao_social"].ToString();
            QrCadastro.Dispose();
            ds_conexao.desconecta();
            return "Aplicação API ao cliente :" + cNome;
        }
    }
}