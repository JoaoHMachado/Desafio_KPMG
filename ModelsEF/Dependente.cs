using System;
using System.Collections.Generic;

namespace WebAppPeopleCRUD.ModelsEF;

public partial class Dependente
{
    public int IdDependente { get; set; }

    public string NomeDependente { get; set; } = null!;

    public string IdadeDependente { get; set; } = null!;

    public int IdPessoa { get; set; }

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;
}
