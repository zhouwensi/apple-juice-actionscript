﻿using System;
using System.Collections.Generic;
using System.Text;



namespace ASCTest
{
    class Program
    {
        

        static void Main(string[] args)
        {
			//ASCompiler.compiler.Builder bu = new ASCompiler.compiler.Builder();
			//byte[] b = bu.BuildLibBin();
			//System.IO.File.WriteAllBytes("astoolglobal.swc", b);



			ASTool.Grammar grammar = ASCompiler.Grammar.getGrammar();

            //string teststring = "package{}var a:String = \"first\";var b:String = \"First\"; var c=a==b;";
            string teststring = "package{}";//System.IO.File.ReadAllText("../../testScript/AS3Testproj/src/Main.as");

            string[] files =null;
            
            
            if (args.Length > 0)
            {
                string path = args[0]; //path = @"F:\ASTool\ASCTest\bin\Release\tests\2_managed_array\";
									   //path = @"F:\ASTool\ASCTest\testScript\AS3Testproj\src\";
				//path = @"E:\Manju-pc\as3protobuf\AS3ProtoBuf\protobuflib";

				if (path.EndsWith(".as"))
                {
                    path = System.IO.Path.GetDirectoryName(path);
                }

                if (string.IsNullOrEmpty(path))
                {
                    path=".\\";
                }

				//path = "";
                //files =new string[] { "E:/Manju-pc/as3protobuf/AS3ProtoBuf/src/com/netease/protobuf/Message.as" };


                string[] ps = path.Split(System.IO.Path.DirectorySeparatorChar);
                if (ps.Length == 2 && string.IsNullOrEmpty(ps[1])  && ps[0].IndexOf( System.IO.Path.VolumeSeparatorChar)>0)
                {
                    Console.WriteLine("无法在根目录下搜索.请将as源代码放到一个文件夹内");
                    return;
                }
                else if (System.IO.Directory.Exists(path))
                {
                    //Console.WriteLine(path);
                    //teststring = System.IO.File.ReadAllText(args[0]);
                    files = System.IO.Directory.GetFiles(path, "*.as", System.IO.SearchOption.AllDirectories );
                }
            }
            else
            {
                Console.Write("输入as文件所在路径");
                return;
            }

            if (files == null)
            {
                Console.Write("输入as文件所在路径");
                return;
            }


			//*********加入API*****
			string apidir = @"F:\ASTOOL\ASTool\LinkCodeGen\bin\Debug\as3api";
			if (System.IO.Directory.Exists(apidir))
			{
				var linkapi = System.IO.Directory.GetFiles(apidir, "*.as", System.IO.SearchOption.AllDirectories);
				string[] n = new string[files.Length + linkapi.Length];
				linkapi.CopyTo(n, 0);
				files.CopyTo(n, linkapi.Length);
				files = n;
			}
			//*********************
			
			var proj = new ASTool.AS3.AS3Proj();
            var srcout = new ASTool.ConSrcOut();

            for (int i = 0; i < files.Length; i++)
            {
                grammar.hasError = false;
                teststring = System.IO.File.ReadAllText(files[i]);
                if (string.IsNullOrEmpty(teststring))
                {
                    continue;
                }

                var tree = grammar.ParseTree(teststring, ASTool.AS3LexKeywords.LEXKEYWORDS , 
                            ASTool.AS3LexKeywords.LEXSKIPBLANKWORDS  ,files[i]);

                //System.IO.File.WriteAllText("d:\\" + System.IO.Path.GetFileName(files[i]), tree.GetTreeString());

                if (grammar.hasError)
                {
                    Console.WriteLine(files[i]);
                    Console.WriteLine("解析语法树失败!");
                    Console.ReadLine();
                    return;
                }

                

                var analyser = new ASTool.AS3FileGrammarAnalyser(proj, files[i]);
                if (!analyser.Analyse(grammar, tree)) //生成项目的语法树
                {
                    Console.WriteLine(analyser.err.ToString());
                    Console.WriteLine("语义分析失败!");
                    Console.ReadLine();
                    return;
                }
#if DEBUG
                //Console.Clear();
#endif
            }

#if DEBUG

            Console.WriteLine();
            Console.WriteLine("====语法树翻译====");

            //runtimeCompiler rtLoader = new runtimeCompiler();
            foreach (var p in proj.SrcFiles)
            {
                p.Write(0, srcout);
            }
            
#endif
            //Console.Read();
            //return;
            ASCompiler.compiler.Builder builder = new ASCompiler.compiler.Builder();
			builder.LoadLibrary( System.IO.File.ReadAllBytes("as3protobuf.swc") );
			//builder.LoadLibrary( System.IO.File.ReadAllBytes("astoolglobal.swc"));

			builder.Build(proj,new ASBinCode.INativeFunctionRegister[] { new extFunctions() } );


            if (builder.buildErrors.Count == 0)
            {
                ASBinCode.CSWC swc = builder.getBuildOutSWC();
				//System.IO.File.WriteAllBytes("astoolglobal.swc", swc.toBytes());
				//System.IO.File.WriteAllBytes("as3protobuf.swc", swc.toBytes());

				if (swc != null)
                {
#if DEBUG
                    
                    for (int i = 0; i < swc.blocks.Count; i++)
                    {
                        var block = swc.blocks[i];
                        if (block != null)
                        {
                            Console.WriteLine();
                            Console.WriteLine("====操作指令 block " + block.name + " " + block.id + "====");
                            Console.WriteLine();
                            Console.WriteLine("total registers:" + block.totalRegisters);
                            Console.WriteLine(block.ToString());
                        }
                    }

#endif

                    if (swc.blocks.Count > 0)
                    {
                        ASRuntime.Player player = new ASRuntime.Player();
                        player.loadCode(swc);

						var d=player.createInstance("SProtoSpace.role_base_info");
						var byteArray = player.createInstance("flash.utils.ByteArray");

						var f = player.getMethod(d, "writeTo");
						var r= player.invokeMethod(d,f,byteArray);
						var d2 = player.createInstance("SProtoSpace.role_base_info");
						var f2 = player.getMethod(d2, "mergeFrom");
						var k = player.invokeMethod(d2, f2, byteArray);




						Console.WriteLine();
                        Console.WriteLine("====程序输出====");

                        player.run(null);



                    }
                    Console.WriteLine();
                }

            }

#if DEBUG
            Console.WriteLine("按任意键结束");
            Console.ReadLine();
#endif

        }
    }
}
