/*
    Copyright 2010 MCLawl Team - Written by Valek (Modified by MCGalaxy)

    Edited for use with MCGalaxy
 
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using System.CodeDom.Compiler;


namespace PattyKaki.Scripting
{
    public sealed class CSCompiler_Simple : ICodeDomCompiler_Simple
    {
        public override string FileExtension { get { return ".cs"; } }
        public override string ShortName { get { return "CS"; } }
        public override string FullName { get { return "CSharp"; } }

        public override CodeDomProvider CreateProvider()
        {

            return CodeDomProvider.CreateProvider("CSharp");
        }

        public override void PrepareArgs(CompilerParameters args)
        {
            args.CompilerOptions += " /unsafe";
        }


        public override string SimplePluginSkeleton
        {
            get
            {
                return @"//This is an example simple plugin source!
using System;
namespace PattyKaki
{{
\tpublic class {0} : Plugin_Simple
\t{{
\t\tpublic override string Name {{ get {{ return ""{0}""; }} }}
\t\tpublic override string Welcome {{ get {{ return ""{0} loaded!""; }} }}
\t\tpublic override int Build {{ get {{ return 0; }} }}
\t\tpublic override string PK_Version {{ get {{ return ""0.0.0.1""; }} }}
\t\tpublic override string Creator {{ get {{ return ""{1}""; }} }}
\t\tpublic override bool LoadAtStartup {{ get {{ return true; }} }}

\t\tpublic override void Load()
\t\t{{
\t\t\t//LOAD YOUR SIMPLE PLUGIN WITH EVENTS OR OTHER THINGS!
\t\t}}
                        
\t\tpublic override void Unload()
\t\t{{
\t\t\t//UNLOAD YOUR SIMPLE PLUGIN BY SAVING FILES OR DISPOSING OBJECTS!
\t\t}}
                        
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\t//HELP INFO!
\t\t}}
\t}}
}}";
            }
        }
    }

    public sealed class VBCompiler_Simple : ICodeDomCompiler_Simple
    {
        public override string FileExtension { get { return ".vb"; } }
        public override string ShortName { get { return "VB"; } }
        public override string FullName { get { return "Visual Basic"; } }

        public override CodeDomProvider CreateProvider()
        {

            return CodeDomProvider.CreateProvider("VisualBasic");
        }

        public override void PrepareArgs(CompilerParameters args) { }
        public override string CommentPrefix { get { return "'"; } }



        public override string SimplePluginSkeleton
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