namespace Chest.Exception;

// 'abstract' porque ninguém deve lançar uma "Exceção Genérica"
public abstract class ChestException : SystemException
{
    protected ChestException(string message) : base(message) { }
}