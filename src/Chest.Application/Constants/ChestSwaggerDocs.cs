namespace Chest.Application.Constants;

public static class ChestSwaggerDocs
{
    public const string CreateSummary = "Cria um novo baú";
    public const string CreateDescription = "Recebe os dados do Jogador 1 para spawnar um baú no mapa com IsSomebodyPlaying = false.";
    
    public const string  FindSummary = "Procura o baú próximo";
    public const string FindDescription = "Recebe a localização do jogador 2 para encontrar o baú mais próximo";
    
    public const string  UpdateSummary = "Atualiza o baú criado";
    public const string UpdateDescription = "Quando o jogador 2 seleciona o baú para encontrá-lo, o servidor precisa atualizar os dados de 'isSomebodyPlaying'para true";

    public const string  GetSummary = "Baús criados pelo usário";
    public const string GetDescription = "Retorna todos os baús com a 'userId' do usuário atreladas";
    
    public const string  DeleteSummary = "Deleta um baú";
    public const string DeleteDescription = "Deleta um baú específico - precisa do'chestId'";

    
}