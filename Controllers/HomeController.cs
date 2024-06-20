using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppPeopleCRUD.DTO;
using WebAppPeopleCRUD.Models;
using WebAppPeopleCRUD.ModelsEF;
using WebAppPeopleCRUD.Repository;

namespace WebAppPeopleCRUD.Controllers
{
    public class HomeController(PessoaRepository repository) : Controller
    {
        public IActionResult Index()
        {
            var pessoas = repository.GetPessoaGeral(); // Obt�m as pessoas com seus dependentes
            return View(pessoas); // Retorna a view com as pessoas encontradas
        }
  
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            var result = repository.DeletaPessoa(Id);

            var pessoas = repository.GetPessoaGeral(); // Obt�m as pessoas com seus dependentes

            if (pessoas == null || pessoas.Count == 0)
            {
                ViewBag.Message = "Nenhuma pessoa encontrada com os dados informados.";
            }

            if (result == "OK")
                ViewBag.Message = "Pessoa removida com sucesso.";
            return View("Index", pessoas);
        }

        [HttpPost]
        public IActionResult Search(string nome, string idade)
        {
            var pessoas = repository.GetPessoa(nome, idade); // Obtem as pessoas com seus dependentes

            if (pessoas == null || pessoas.Count == 0)
            {
                ViewBag.Message = "Nenhuma pessoa encontrada com os dados informados.";
            }

            return View(pessoas); // Retorna a view com as pessoas encontradas
        }
        
        [HttpGet]
        public IActionResult Search()
        {
       
            return View("Search"); // Retorna a view com as pessoas encontradas
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var pessoa = repository.GetPessoaById(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PessoaDTO pessoa)
        {
            // Verifica se h� dependentes e se as idades s�o v�lidas
            if (pessoa.Dependentes != null)
            {
                foreach (var dependente in pessoa.Dependentes)
                {
                    if (string.IsNullOrEmpty(dependente.IdadeDependente) ||
                        !int.TryParse(dependente.IdadeDependente, out int idade) || idade < 18)
                    {
                        ModelState.AddModelError("Dependentes",
                            "Todos os dependentes devem ter uma idade v�lida e ser maiores de 18 anos.");
                        break;
                    }
                }
            }

            var dependentes = pessoa.Dependentes.Select(d => (d.NomeDependente, d.IdadeDependente)).ToList();
            var result = repository.UpdatePessoa(pessoa.IdPessoa, pessoa.NomePessoa, pessoa.IdadePessoa, dependentes);

            if (result)
            {
                return RedirectToAction("Index"); // Redireciona para a tela principal
            }
            else
            {
                return View(pessoa);
            }
        }
        
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(string nome, string idade, List<string> dependenteNomes, List<string> dependenteIdades)
        {
            var dependentes = new List<(string Nome, string Idade)>();
            if (dependenteNomes != null && dependenteIdades != null)
            {
                for (int i = 0; i < dependenteNomes.Count; i++)
                {
                    dependentes.Add((dependenteNomes[i], dependenteIdades[i]));
                }
            }
 
            var result = repository.InserirPessoaEDependentes(nome, idade, dependentes);

            ViewBag.Message = result;
            return View();
        }
        
    }
}