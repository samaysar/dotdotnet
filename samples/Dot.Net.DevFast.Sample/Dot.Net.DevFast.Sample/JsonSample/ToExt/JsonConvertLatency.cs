using System;
using System.Diagnostics;
using System.Text;
using Dot.Net.DevFast.Extensions.JsonExt;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Sample.JsonSample.ToExt
{
    public static class JsonConvertLatency
    {
        public static void Run()
        {
            Console.Out.WriteLine("-------SmallObj Serialization-------");
            //Small Object serialization in 10M loops
            Run(10 * 1024 * 1024, new SmallObj
            {
                Address = "123, Json street",
                Age = 20,
                Name = "Json Born"
            });
            Console.Out.WriteLine("-------LargeObj Serialization-------");
            //Large Object serialization in 1M loops
            Run(1024 * 1024, LargeObj);

            Console.Out.WriteLine("-------LargeObj Array Serialization-------");
            //creating array of 1K LargeObj
            var objArr = new LargeObj[1024];
            for (var i = 0; i < 1024; i++)
            {
                objArr[i] = LargeObj;
            }
            //Large Object Array serialization in 1K loops
            Run(1024, objArr);
        }

        private static void Run(int iteration, object data)
        {
            //warm up
            data.MeasureJsonConvert(2, false);
            var jsonTime = data.MeasureJsonConvert(iteration);

            GC.Collect();
            GC.WaitForFullGCApproach();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForFullGCApproach();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();

            var reusableStringBuilder = new StringBuilder();
            //warm up
            data.MeasureDevFast(reusableStringBuilder, 2, false);
            var devfastTime = data.MeasureDevFast(reusableStringBuilder, iteration);
            var dfFastness = ((int) ((100 - (devfastTime / jsonTime * 100)) * 100)) / 100.0;
            Console.Out.WriteLine("DevFast " + Math.Abs(dfFastness) + (dfFastness < 0 ? " % Slower" : " % Faster"));
            Console.Out.WriteLine();
        }

        private static double MeasureJsonConvert(this object obj, int iteration, bool print = true)
        {
            var json = string.Empty;
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iteration; i++)
            {
                json = JsonConvert.SerializeObject(obj);
            }
            sw.Stop();

            if (print)
            {
                Console.Out.WriteLine("JsonConvert Total Time: " + sw.Elapsed.TotalMilliseconds);
            }
            else
            {
                Console.Out.WriteLine("StringLen: " + json.Length);
            }
            return sw.Elapsed.TotalMilliseconds;
        }

        private static double MeasureDevFast(this object obj, StringBuilder sb, int iteration, bool print = true)
        {
            var json = string.Empty;
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iteration; i++)
            {
                obj.ToJson(sb, new JsonSerializer());
                json = sb.ToString();
                sb.Clear();
            }
            sw.Stop();

            if (print)
            {
                Console.Out.WriteLine("DevFast Total Time: " + sw.Elapsed.TotalMilliseconds);
            }
            else
            {
                Console.Out.WriteLine("StringLen: " + json.Length);
            }
            return sw.Elapsed.TotalMilliseconds;
        }

        private static readonly LargeObj LargeObj = new LargeObj
        {
            Address = "123, Json street",
            Age = 20,
            Name = "Json Born",
            City = "Llanfair­pwllgwyn­gyllgo­gerychwyrn­drobwll­llanty­silio­gogogoch",
            Country = "The Former Yugoslav Republic of Macedonia",
            AboutMe =
                @"Do play they miss give so up. Words to up style of since world. We leaf to snug on no need. Way own uncommonly travelling now acceptance bed compliment solicitude. Dissimilar admiration so terminated no in contrasted it. Advantages entreaties mr he apartments do. Limits far yet turned highly repair parish talked six. Draw fond rank form nor the day eat.
Her extensive perceived may any sincerity extremity. Indeed add rather may pretty see. Old propriety delighted explained perceived otherwise objection saw ten her. Doubt merit sir the right these alone keeps. By sometimes intention smallness he northward. Consisted we otherwise arranging commanded discovery it explained. Does cold even song like two yet been. Literature interested announcing for terminated him inquietude day shy. Himself he fertile chicken perhaps waiting if highest no it. Continued promotion has consulted fat improving not way.
Excited him now natural saw passage offices you minuter. At by asked being court hopes. Farther so friends am to detract. Forbade concern do private be. Offending residence but men engrossed shy. Pretend am earnest offered arrived company so on. Felicity informed yet had admitted strictly how you.
Your it to gave life whom as. Favourable dissimilar resolution led for and had. At play much to time four many. Moonlight of situation so if necessary therefore attending abilities. Calling looking enquire up me to in removal. Park fat she nor does play deal our. Procured sex material his offering humanity laughing moderate can. Unreserved had she nay dissimilar admiration interested. Departure performed exquisite rapturous so ye me resources.
Manor we shall merit by chief wound no or would. Oh towards between subject passage sending mention or it. Sight happy do burst fruit to woody begin at. Assurance perpetual he in oh determine as. The year paid met him does eyes same. Own marianne improved sociable not out. Thing do sight blush mr an. Celebrated am announcing delightful remarkably we in literature it solicitude. Design use say piqued any gay supply. Front sex match vexed her those great.
Unfeeling so rapturous discovery he exquisite. Reasonably so middletons or impression by terminated. Old pleasure required removing elegance him had. Down she bore sing saw calm high. Of an or game gate west face shed. ﻿no great but music too old found arose.
Kindness to he horrible reserved ye. Effect twenty indeed beyond for not had county. The use him without greatly can private. Increasing it unpleasant no of contrasted no continuing. Nothing colonel my no removed in weather. It dissimilar in up devonshire inhabiting.
He unaffected sympathize discovered at no am conviction principles. Girl ham very how yet hill four show. Meet lain on he only size. Branched learning so subjects mistress do appetite jennings be in. Esteems up lasting no village morning do offices. Settled wishing ability musical may another set age. Diminution my apartments he attachment is entreaties announcing estimating. And total least her two whose great has which. Neat pain form eat sent sex good week. Led instrument sentiments she simplicity.
Old there any widow law rooms. Agreed but expect repair she nay sir silent person. Direction can dependent one bed situation attempted. His she are man their spite avoid. Her pretended fulfilled extremely education yet. Satisfied did one admitting incommode tolerably how are.
He difficult contented we determine ourselves me am earnestly. Hour no find it park. Eat welcomed any husbands moderate. Led was misery played waited almost cousin living. Of intention contained is by middleton am. Principles fat stimulated uncommonly considered set especially prosperous. Sons at park mr meet as fact like."
        };
    }
}