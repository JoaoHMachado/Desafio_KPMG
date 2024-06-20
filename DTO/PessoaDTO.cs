using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using WebAppPeopleCRUD.ModelsEF;

namespace WebAppPeopleCRUD.DTO
{
    public class PessoaDTO
    {
        public int IdPessoa { get; set; }
        public string NomePessoa { get; set; }
        public string  IdadePessoa { get; set; }
        public List<DependenteDTO> Dependentes { get; set; } = new List<DependenteDTO>();
    }

    public class DependenteDTO
    {
        public string IdDependente { get; set; }

        [Required(ErrorMessage = "O nome do dependente é obrigatório.")]
        public string NomeDependente { get; set; }

        [Required(ErrorMessage = "A idade do dependente é obrigatória.")]
        [Range(18, int.MaxValue, ErrorMessage = "A idade do dependente deve ser 18 anos ou mais.")]
        public string IdadeDependente { get; set; }

        public DateTime datNascimento { get; set; }
        public DateTime dataAtual { get; set; }

        public int idade { get; set; }
    }



}
