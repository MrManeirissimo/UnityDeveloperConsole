using System.Collections.Generic;
using System.Collections;
using System.Text;

using UnityEngine;
using CommandLine;

public class WR : System.IO.TextWriter {
    public override Encoding Encoding => Encoding.UTF8;

    public override void WriteLine(string value) {
        Debug.Log(value);
    }
}

public class Sample : MonoBehaviour {
    WR wR;
    public TMPro.TextMeshProUGUI text;

    ParserResult<OptionsOne> parserResult;

    class Options {
        [Value(0)]
        public int IntValue {
            get;
            set;
        }

        [Value(1, Min = 1, Max = 3)]
        public IEnumerable<string> StringSeq {
            get;
            set;
        }

        [Value(2)]
        public double DoubleValue {
            get;
            set;
        }
    }

    public class OptionsOne {

        //[Option('g', "gravity")]
        //public int Grav { get; set; }

        //[Value(1, HelpText = "Sets output to verbose message.")]
        //public bool Verbose { get; set; }

        [Value(0, HelpText = "Sets the gravity.")]
        public int Grav { get; set; }

        //[Option('v', "verbose", Required = false, HelpText = "Sets output to verbose message.")]
        //public bool Verbose { get; set; }
    }

    [Verb("default")]
    public class VerbOne {
        [Option('v', "verbose", Required = false, HelpText = "Sets output to verbose message.")]
        public bool Verbose { get; set; }

        [Option('g', "gravity", Required = false, HelpText = "Sets the gravity.")]
        public float Grav { get; set; }
    }

    private void Awake() {
        wR = new WR();
    }

    [ContextMenu("Go")]
    private void Start() {
        var parser = new Parser(config => config.HelpWriter = wR);

        var args = "10 str1 str2 str3 1.1".Split();
        var res = Parser.Default.ParseArguments<Options>(args);
        res.WithParsed(options => {
            Debug.Log("Parsed");
        });
        res.WithNotParsed(options => {
            Debug.Log("Failed");
        });

        //var result = Parser.Default.ParseArguments(new string[] { "default", "v", "g", "5" }, typeof(VerbOne));
        //var str = HelpText.AutoBuild(result, h => h, e => e);
        //text.text = str.ToString();


        //result.WithParsed(options => {
        //    VerbOne verb = (VerbOne)options;
        //    Debug.Log("Parsed");
        //});
        //result.WithNotParsed(options => {
        //    Debug.Log("Failed");
        //});
    }
}
