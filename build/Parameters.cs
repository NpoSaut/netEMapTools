using CommandLine;

public class Parameters
{
    [Option('o', "publishplace", Required = false, Default = null, HelpText = "Place to publish the installer.")]
    public string PublishPlace { get; set; }

    [Option('c', "configuration", Required = false, Default = "Release", HelpText = "Configuration to publish.")]
    public string Configuration { get; set; }
}