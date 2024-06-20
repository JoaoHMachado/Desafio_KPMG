using System.Data;
using Microsoft.Data.SqlClient;
using WebAppPeopleCRUD.DTO;

namespace WebAppPeopleCRUD.Repository;
public class PessoaRepository
{
    public bool UpdatePessoa(int id, string Nome, string Idade, List<(string Nome, string Idade)> Dependentes)
    {
        using var connection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
        try
        { 
            connection.Open();

            using (var cmd = new SqlCommand("SP_Atualiza_Pessoa", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPessoa", id);
                cmd.ExecuteNonQuery();
            }

            InserirPessoaEDependentes(Nome, Idade, Dependentes);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public string InserirPessoaEDependentes(string Nome, string Idade, List<(string Nome, string Idade)> Dependentes)
    {
        using var connection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
        try
        {
            connection.Open();

            // Passo 1: Inserir a Pessoa e obter o IdPessoa inserido
            int idPessoa = 0;
            using (var cmd = new SqlCommand("SP_Inseri_Pessoa", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NomePessoa", Nome);
                cmd.Parameters.AddWithValue("@IdadePessoa", Idade);
                cmd.Parameters.Add("@IdPessoa", SqlDbType.Int).Direction =
                    ParameterDirection.Output; // Parâmetro de saída

                cmd.ExecuteNonQuery();

                idPessoa = Convert.ToInt32(cmd.Parameters["@IdPessoa"].Value); // Obtém o IdPessoa inserido
            }

            // Passo 2: Inserir os Dependentes associados à Pessoa
            foreach (var dep in Dependentes)
            {
                using (var cmd = new SqlCommand("SP_Inseri_Dependente", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPessoa", idPessoa);
                    cmd.Parameters.AddWithValue("@NomeDependente", dep.Nome);
                    cmd.Parameters.AddWithValue("@IdadeDependente", dep.Idade);

                    cmd.ExecuteNonQuery();
                }
            }

            return "Cadastro realizado com sucesso.";
        }
        catch (Exception ex)
        {
            return $"Erro ao cadastrar pessoa e dependentes: {ex.Message}";
        }
    }

    public List<PessoaDTO> GetPessoa(string nome, string idade)
    {
        List<PessoaDTO> pessoas = new List<PessoaDTO>();

        try
        {
            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING")))
            {
                connection.Open();

                using (var cmd = new SqlCommand("SP_Busca_Pessoa", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NomePessoa", nome);
                    cmd.Parameters.AddWithValue("@IdadePessoa", idade);

                    using (var reader = cmd.ExecuteReader())
                    {
                        // Enquanto houver registros para ler
                        while (reader.Read())
                        {
                            int idPessoa = Convert.ToInt32(reader["IdPessoa"]);
                            string nomePessoa = reader["NomePessoa"].ToString();
                            DateTime datNascimento = Convert.ToDateTime(reader["IdadePessoa"]);
                            string idadePessoa = CalcularIdade(datNascimento).ToString();

                            PessoaDTO pessoa = pessoas.Find(p => p.IdPessoa == idPessoa);

                            if (pessoa == null)
                            {
                                pessoa = new PessoaDTO
                                {
                                    IdPessoa = idPessoa,
                                    NomePessoa = nomePessoa,
                                    IdadePessoa = idadePessoa,
                                    Dependentes = new List<DependenteDTO>()
                                };

                                pessoas.Add(pessoa);
                            }

                            var x = reader["IdadeDependente"].ToString();
                            string idadeDependente = "";
                            if (x != "")
                            {
                                DateTime? dataDependente = Convert.ToDateTime(reader["IdadeDependente"]) as DateTime?;
                                idadeDependente = dataDependente.HasValue
                                    ? CalcularIdade(dataDependente.Value).ToString()
                                    : "N/A";
                            }
                            else
                            {
                                idadeDependente = x;
                            }
                            // Adiciona o dependente para esta pessoa

                            pessoa.Dependentes.Add(new DependenteDTO
                            {
                                IdDependente = reader["IdDependente"].ToString(),
                                NomeDependente = reader["NomeDependente"].ToString(),
                                IdadeDependente = idadeDependente,
                            });
                        }
                    }
                }
            }

            return pessoas;
        }
        catch (Exception ex)
        {
            // Trate o erro conforme necessário
            throw new Exception($"Erro ao consultar pessoas: {ex.Message}");
        }
    }

    public List<PessoaDTO> GetPessoaGeral()
    {
        List<PessoaDTO> pessoas = new List<PessoaDTO>();

        try
        {
            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING")))
            {
                connection.Open();

                using (var cmd = new SqlCommand("SP_BuscaGeral_Pessoa", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        // Enquanto houver registros para ler
                        while (reader.Read())
                        {
                            int idPessoa = Convert.ToInt32(reader["IdPessoa"]);
                            string nomePessoa = reader["NomePessoa"].ToString();
                            DateTime datNascimento = Convert.ToDateTime(reader["IdadePessoa"]);
                            string idadePessoa = CalcularIdade(datNascimento).ToString();

                            PessoaDTO pessoa = pessoas.Find(p => p.IdPessoa == idPessoa);

                            if (pessoa == null)
                            {
                                pessoa = new PessoaDTO
                                {
                                    IdPessoa = idPessoa,
                                    NomePessoa = nomePessoa,
                                    IdadePessoa = idadePessoa,
                                    Dependentes = new List<DependenteDTO>()
                                };

                                pessoas.Add(pessoa);
                            }

                            // Adiciona o dependente para esta pessoa
                            pessoa.Dependentes.Add(new DependenteDTO
                            {
                                IdDependente = reader["IdDependente"].ToString(),
                                NomeDependente = reader["NomeDependente"].ToString(),
                                IdadeDependente = CalcularIdade(Convert.ToDateTime(reader["IdadeDependente"]))
                                    .ToString(),
                            });
                        }
                    }
                }
            }

            return pessoas;
        }
        catch (Exception ex)
        {
            // Trate o erro conforme necessário
            throw new Exception($"Erro ao consultar pessoas: {ex.Message}");
        }
    }

    public PessoaDTO GetPessoaById(int id)
    {
        PessoaDTO pessoa = null;

        try
        {
            var connection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            connection.Open();

            using (var cmd = new SqlCommand("SP_Busca_Pessoa_Por_Id", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPessoa", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pessoa = new PessoaDTO
                        {
                            IdPessoa = id,
                            NomePessoa = reader["NomePessoa"].ToString(),
                            IdadePessoa = reader["IdadePessoa"].ToString(),
                            Dependentes = new List<DependenteDTO>()
                        };

                        do
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("IdDependente")))
                            {
                                var idDependente = reader["IdDependente"].ToString();
                                var nomeDependente = reader["NomeDependente"].ToString();
                                var idadeDependente = reader["IdadeDependente"].ToString();

                                pessoa.Dependentes.Add(new DependenteDTO
                                {
                                    IdDependente = idDependente,
                                    NomeDependente = nomeDependente,
                                    IdadeDependente = idadeDependente
                                });
                            }
                        } while (reader.Read());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao consultar pessoa: {ex.Message}");
        }

        return pessoa;
    }

    public int CalcularIdade(DateTime dataNascimento)
    {
        DateTime dataAtual = DateTime.Today;
        int idade = dataAtual.Year - dataNascimento.Year;

        // Verifica se ainda não fez aniversário este ano
        if (dataNascimento.Date > dataAtual.AddYears(-idade))
        {
            idade--;
        }

        return idade;
    }


    public string DeletaPessoa(int Id)
    {
        using var connection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
        try
        {
            connection.Open();
            using var cmd = new SqlCommand("SP_Deleta_Pessoa", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdPessoa", Id);
            cmd.ExecuteNonQuery();
            return "OK";
        }
        catch (Exception ex)
        {
            return $"Erro ao atualizar dados: {ex.Message}";
        }
    }
}