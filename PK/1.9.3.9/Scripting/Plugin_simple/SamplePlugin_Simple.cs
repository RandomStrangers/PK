namespace PattyKaki.Scripting.Plugin_simple
{
    public class SamplePlugin_Simple
    {
        public string SimplePluginSkeleton
        {
            get
            {
                return @"' This is an example simple plugin source!
Imports System

Namespace PattyKaki
\tPublic Class {0}
\t\tInherits Plugin_Simple

\t\tPublic Overrides ReadOnly Property Name() As String
\t\t\tGet
\t\t\t\tReturn ""{0}""
\t\t\tEnd Get
\t\t End Property
\t\tPublic Overrides ReadOnly Property Welcome() As String
\t\t\tGet
\t\t\t\tReturn ""{0} loaded!""
\t\t\tEnd Get
\t\t End Property
\t\tPublic Overrides ReadOnly Property Build() As Integer
\t\t\tGet
\t\t\t\tReturn 0
\t\t\tEnd Get
\t\t End Property
\t\tPublic Overrides ReadOnly Property PK_Version() As String
\t\t\tGet
\t\t\t\tReturn ""0.0.0.1""
\t\t\tEnd Get
\t\t End Property
\t\tPublic Overrides ReadOnly Property Creator() As String
\t\t\tGet
\t\t\t\tReturn ""{1}""
\t\t\tEnd Get
\t\t End Property
\t\tPublic Overrides ReadOnly Property LoadAtStartup() As Boolean
\t\t\tGet
\t\t\t\tReturn True
\t\t\tEnd Get
\t\t End Property


\t\tPublic Overrides Sub Load()
\t\t\t' LOAD YOUR SIMPLE PLUGIN WITH EVENTS OR OTHER THINGS!
\t\tEnd Sub
                        
\t\tPublic Overrides Sub Unload()
\t\t\t' UNLOAD YOUR SIMPLE PLUGIN BY SAVING FILES OR DISPOSING OBJECTS!
\t\tEnd Sub
                        
\t\tPublic Overrides Sub Help(p As Player)
\t\t\t' HELP INFO!
\t\tEnd Sub
\tEnd Class
End Namespace";
            }
        }
    }
}