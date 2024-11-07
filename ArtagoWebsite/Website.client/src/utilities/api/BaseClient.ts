export default abstract class BaseClass
{
    transformOptions(options: RequestInit)
    {
        return Promise.resolve(options);
    }
}