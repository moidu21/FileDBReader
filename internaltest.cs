﻿using FileDBReader.src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileDBReader
{
    class internaltest {

        static String TEST_DIRECTORY_NAME = "tests";
        static String FILEFORMAT_DIRECTORY_NAME = "FileFormats";

        static FileReader reader = new FileReader();
        static XmlExporter exporter = new XmlExporter();
        static FileWriter writer = new FileWriter();
        static XmlInterpreter interpreter = new XmlInterpreter();
        static ZlibFunctions zlib = new ZlibFunctions();

        public static void Main(String[] args)
        {
            IslandTestGoodwill();
        }

        public static void ListTest() {

            String DIRECTORY_NAME = "lists";
            String INTERPRETER_FILE = Path.Combine(FILEFORMAT_DIRECTORY_NAME, "Island_Gamedata.xml"); 

            String TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata_og.data");
            String DECOMPRESSED_TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata_decompressed.xml");
            String INTERPRETED_TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata_interpreted.xml");
            String TOHEX_TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata_backtohex.xml");
            String EXPORTED_TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata.data");

            //decompress gamedata.data
            var interpreterDoc = new XmlDocument();
            interpreterDoc.Load(INTERPRETER_FILE);

            //decompress
            var decompressed = reader.ReadFile(TESTFILE);
            decompressed.Save(DECOMPRESSED_TESTFILE);

            //interpret
            var interpreted = interpreter.Interpret(decompressed, interpreterDoc);
            interpreted.Save(INTERPRETED_TESTFILE);

            //to hex 
            var Hexed = exporter.Export(interpreted, interpreterDoc);
            Hexed.Save(TOHEX_TESTFILE);

            //back to gamedata 
            writer.Export(Hexed, EXPORTED_TESTFILE);
        }


        /// <summary>
        /// Test for the two island interpreters
        /// </summary>
        public static void IslandTest() {
            IslandTestGamedata();
            IslandTestRd3d();
            IslandTestTMC();
        }


        private static void IslandTestGoodwill()
        {
            //test directory
            String DIRECTORY_NAME = "goodwill";
            //interpreter file path
            String INTERPRETER_GAMEDATA = Path.Combine(FILEFORMAT_DIRECTORY_NAME, "internalfiledbtest.xml");
            //input file path
            String GAMEDATA_FILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata_og.data");
            //output file path
            String GAMEDATA_INTERPRETED_PATH = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "Island_Gamedata_interpreted.xml");
            String GAMEDATA_READ_PATH = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "Island_Gamedata_Read.xml");
            String GAMEDATA_EXPORTED_PATH = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "Island_Gamedata_exported.xml");
            //create interpreter document
            var GamedataInterpreter = new XmlDocument();
            GamedataInterpreter.Load(INTERPRETER_GAMEDATA);

            //decompress interpret and save gamedata.data
            var doc = reader.ReadFile(GAMEDATA_FILE);
            doc.Save(GAMEDATA_READ_PATH);
            var interpreted_gamedata = interpreter.Interpret(reader.ReadFile(GAMEDATA_FILE), GamedataInterpreter);
            interpreted_gamedata.Save(GAMEDATA_INTERPRETED_PATH);

            var exported = exporter.Export(interpreted_gamedata, GamedataInterpreter);
            exported.Save(GAMEDATA_EXPORTED_PATH);



        }

        private static void IslandTestGamedata() {
            //test directory
            String DIRECTORY_NAME = "island";
            //interpreter file path
            String INTERPRETER_GAMEDATA = Path.Combine(FILEFORMAT_DIRECTORY_NAME, "Island_Gamedata.xml");
            //input file path
            String GAMEDATA_FILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata.data");
            //output file path
            String GAMEDATA_INTERPRETED_PATH = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "Island_Gamedata_interpreted.xml");
            //create interpreter document
            var GamedataInterpreter = new XmlDocument();
            GamedataInterpreter.Load(INTERPRETER_GAMEDATA);

            //decompress interpret and save gamedata.data
            var interpreted_gamedata = interpreter.Interpret(reader.ReadFile(GAMEDATA_FILE), GamedataInterpreter);
            interpreted_gamedata.Save(GAMEDATA_INTERPRETED_PATH);
        }

        private static void IslandTestTMC() 
        {
            //test directory
            String DIRECTORY_NAME = "island";
            //interpreter file path
            String INTERPRETER_GAMEDATA = Path.Combine(FILEFORMAT_DIRECTORY_NAME, "tmc.xml");
            //input file path
            String GAMEDATA_FILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "0x0.tmc");
            //output file path
            String GAMEDATA_INTERPRETED_PATH = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "0x0.xml");
            //create interpreter document
            var GamedataInterpreter = new XmlDocument();
            GamedataInterpreter.Load(INTERPRETER_GAMEDATA);

            //decompress interpret and save gamedata.data
            var interpreted_gamedata = interpreter.Interpret(reader.ReadFile(GAMEDATA_FILE), GamedataInterpreter);
            interpreted_gamedata.Save(GAMEDATA_INTERPRETED_PATH);
        }

        private static void IslandTestRd3d()
        {
            //test directory
            String DIRECTORY_NAME = "island";
            //interpreter file path
            String INTERPRETER_RD3D = Path.Combine(FILEFORMAT_DIRECTORY_NAME, "Island_Rd3d.xml");
            //input file path
            String RD3D_FILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "rd3d.data");
            //output file path
            String RD3D_INTERPRETED_PATH = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "Island_Rd3d_interpreted.xml");
            //create interpreter document
            var GamedataInterpreter = new XmlDocument();
            GamedataInterpreter.Load(INTERPRETER_RD3D);

            //decompress interpret and save the resulting document
            var interpreted_gamedata = interpreter.Interpret(reader.ReadFile(RD3D_FILE), GamedataInterpreter);
            interpreted_gamedata.Save(RD3D_INTERPRETED_PATH);
        }

        /// <summary>
        /// Test for the ctt interpreter.
        /// </summary>
        public static void CttTest() {
            const String DIRECTORY_NAME = "ctt";
            const String INTERPRETER_FILE = "FileFormats/ctt.xml";
            
            var interpreterDoc = new XmlDocument();
            interpreterDoc.Load(INTERPRETER_FILE);

            FileStream fs = File.OpenRead(Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "0x1.ctt"));

            //Ubisoft uses 8 magic bytes at the start
            var doc = interpreter.Interpret(reader.ReadSpan(zlib.Decompress(fs, 8)), interpreterDoc);
            doc.Save(Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "interpreted.xml"));

        }

        /// <summary>
        /// Test for DEFLATE/zlib implementation. 
        /// decompresses 0x1.ctt, ignoring the 8 magic bytes at the start, writes the result to decompressed.xml, then compresses it back.
        /// </summary>
        public static void zlibTest() {
            const String DIRECTORY_NAME = "zlib";

            FileStream fs = File.OpenRead(Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "0x1.ctt"));

            //Ubisoft uses 8 magic bytes at the start
            var doc = reader.ReadSpan(zlib.Decompress(fs, 8));
            doc.Save(Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "decompressed.xml"));

            var Stream = writer.Export(doc, ".bin");
            File.WriteAllBytes(Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "shittycompress.ctt"), zlib.Compress(Stream, 1));
        }

        /// <summary>
        /// decompresses the two files original.bin and recompressed.bin which are preextracted inner filedb files from gamedata_og.data
        /// </summary>
        public static void InnerFileDBTest() {
            const String DIRECTORY_NAME = "filedb";

            var reader = new FileReader();
            reader.ReadFile(Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "original.bin")).Save(Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "original.xml"));
            reader.ReadFile(Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "recompressed.bin")).Save(Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "recompressed.xml"));
        }

        /// <summary>
        /// Decompresses gamedata_og.data, inteprets it with internalfiledbtest.xml, converts it back to hex using the same interpreter, exports back to filedb compression. 
        /// A file is saved at each stage of the process.
        /// </summary>
        public static void CompressionTest() {

            const String DIRECTORY_NAME = "innerfiledb";
            const String INTERPRETER_FILE = "FileFormats/internalfiledbtest.xml";

            String TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata_og.data");
            String DECOMPRESSED_TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata_decompressed.xml");
            String INTERPRETED_TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata_interpreted.xml");
            String TOHEX_TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata_backtohex.xml");
            String EXPORTED_TESTFILE = Path.Combine(TEST_DIRECTORY_NAME, DIRECTORY_NAME, "gamedata.data");

            //decompress gamedata.data
            var interpreterDoc = new XmlDocument();
            interpreterDoc.Load(INTERPRETER_FILE);

            //decompress
            var decompressed = reader.ReadFile(TESTFILE);
            decompressed.Save(DECOMPRESSED_TESTFILE);

            //interpret
            var interpreted = interpreter.Interpret(decompressed, interpreterDoc);
            interpreted.Save(INTERPRETED_TESTFILE);

            //to hex 
            var Hexed = exporter.Export(interpreted, interpreterDoc);
            Hexed.Save(TOHEX_TESTFILE);

            //back to gamedata 
            writer.Export(Hexed, EXPORTED_TESTFILE);
        }
    }
    
}