using System;
using System.Collections.Generic;

namespace WebAppPeopleCRUD.ModelsEF;

public partial class Pessoa
{
    public int IdPessoa { get; set; }

    public string NomePessoa { get; set; } = null!;

    public string IdadePessoa { get; set; } = null!;

    public virtual ICollection<Dependente> Dependentes { get; set; } = new List<Dependente>();
}
