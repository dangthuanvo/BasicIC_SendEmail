namespace BasicIC_SendEmail.Services.Prototype
{
    /// <summary>
    /// Description: Build-up interfaces to handle features regarding clone object
    /// </summary>
    public interface IPrototype
    {
        IPrototype Clone();
        IPrototype DeepClone();
    }
}