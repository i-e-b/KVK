﻿using System;
using KVK.Core.Trie;
using NUnit.Framework;

namespace Kvk.Unit.Tests.TrieTests
{
	[TestFixture, Explicit]
	public class PerformanceTests
	{
		ITrie<object> subject;

		[SetUp]
		public void a_trie_of_objects()
		{
			subject = new Trie<object>();
		}

		[Test][Description("Test bi-direction links in Trie to recover paths")]
		public void querying_node_path_100_000_times ()
		{
			ITrieNode<object> lastNode = null;
			foreach (var word in doc.Split(' ', '\r', '\n'))
			{
				lastNode = subject.Add(word, Guid.Empty);
			}

			var start = DateTime.Now;
			for (int i = 0; i < 1e5; i++)
			{
				subject.GetKey(lastNode);
			}
			var time = DateTime.Now - start;

			Console.WriteLine(time);
			Assert.That(time.TotalSeconds, Is.LessThan(60));
		}

		[Test][Description("Test bi-direction links in Trie to recover paths")]
		public void querying_node_path_by_value_100_000_times ()
		{
			foreach (var word in doc.Split(' ', '\r', '\n'))
			{
				subject.Add(word, Guid.Empty);
			}

			var id = Guid.NewGuid();
			subject.Add("Mauris", id);

			var start = DateTime.Now;
			for (int i = 0; i < 1e5; i++)
			{
				subject.GetKeyByValue(id);
			}
			var time = DateTime.Now - start;

			Console.WriteLine(time);
			Assert.That(time.TotalSeconds, Is.LessThan(60));
		}

		[Test][Description("Takes 20 to 40 character keys from Lorem text and writes them to the Trie")]
		public void inserting_one_million_similar_random_strings()
		{
			var start = DateTime.Now;
			var hlen = (int)(doc.Length * 0.7);
			for (int i = 0; i < 1e6; i++)
			{
				subject.Add(doc.Substring(i % hlen, (i % 20) + 20), this);
			}
			var time = DateTime.Now - start;

			Console.WriteLine(time);
			Assert.That(time.TotalSeconds, Is.LessThan(60), "takes about 4.8s on my laptop");
		}

		[Test][Description("Stresses the AllSubstringValues method, should be light on memory")]
		public void inserting_a_range_of_strings_and_querying_against_a_large_document()
		{
			var start = DateTime.Now;
			foreach (var word in doc.Split(' ', '\r', '\n'))
			{
				subject.Add(word, Guid.Empty);
				subject.AllSubstringValues(doc);
			}
			var time = DateTime.Now - start;

			Console.WriteLine(time);
			Assert.That(time.TotalSeconds, Is.LessThan(60), "about 0.2s on my laptop");
		}
		#region var doc = "..."
		const string doc = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur ut odio odio, placerat interdum lectus. Donec vitae nisl tortor, sed lacinia felis. Sed porta magna volutpat quam varius placerat. Morbi congue, arcu vel tristique commodo, mi velit aliquet nunc, sed ultricies dolor ligula nec magna. Integer fermentum lacinia tempus. Vestibulum pretium, arcu vel vehicula blandit, diam ligula convallis arcu, quis porta turpis orci et nisl. Aliquam eleifend, magna et dignissim pharetra, lacus est sollicitudin neque, a vehicula dolor nunc congue ante. Nullam varius ultrices vehicula. Praesent laoreet pulvinar venenatis. Nam semper ante sit amet nisl imperdiet ut blandit libero eleifend. Sed aliquet suscipit vestibulum. Quisque mattis rhoncus dictum. Praesent ipsum nisl, commodo vel pellentesque ut, imperdiet ac metus. Sed dui urna, aliquam quis venenatis nec, blandit id ante. Phasellus congue tincidunt sem, vitae molestie lorem consequat a.

Vestibulum enim eros, molestie eget lacinia eu, adipiscing vitae diam. Aliquam at nulla est. Morbi at sapien fringilla elit rhoncus elementum. In sed magna vel dolor accumsan eleifend. Sed nulla neque, rhoncus non dapibus consectetur, lacinia ut ipsum. Integer vel leo magna, vitae volutpat nunc. Nunc consectetur tortor nec mauris pretium viverra. Fusce ac massa metus. Nam vitae pretium nibh. Suspendisse nec justo in ipsum mattis convallis. Donec eget elit ut metus adipiscing accumsan vel semper nisi. Cras vehicula est sem. Nullam tincidunt mi nec mauris volutpat adipiscing. Sed sodales, dui quis malesuada ullamcorper, risus ligula lacinia magna, ut consequat turpis turpis a mauris. Vivamus vitae mi nec massa convallis dignissim in in nisi.

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras at turpis quis turpis accumsan consectetur vitae eget mauris. Nulla sodales justo non diam hendrerit sit amet vestibulum erat porttitor. In vehicula aliquet turpis, eu ultricies libero vehicula sed. Suspendisse potenti. Nulla ut vehicula elit. Aenean varius viverra ante consectetur pulvinar. In congue sem vitae ante dictum vel vehicula mi placerat. Vivamus luctus quam sed libero luctus cursus. Sed tristique vulputate velit, et venenatis metus fermentum a. Pellentesque vitae sapien urna. In sapien augue, ultrices egestas semper id, fermentum quis sapien. Aenean sollicitudin libero ac ipsum lobortis quis rutrum nulla congue. Etiam ac sapien lectus. Donec tempor varius consectetur. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.

Nunc pharetra felis sed ante facilisis laoreet. Mauris ac ante enim, id suscipit enim. Sed vel arcu enim. Maecenas aliquam volutpat purus ut semper. Quisque sed ullamcorper ante. Ut vel placerat nisi. Etiam nec aliquam lectus. Aliquam diam turpis, vehicula id volutpat dictum, consectetur id eros. Phasellus ac massa et ipsum ullamcorper vehicula nec et mi. Aliquam erat volutpat. Cras at arcu dui.

Nullam eget elit magna, ut viverra magna. Suspendisse ultricies auctor orci at fringilla. Suspendisse potenti. Maecenas nec odio eu tellus suscipit pretium interdum et lacus. Praesent iaculis magna eget augue convallis eget elementum felis blandit. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Aliquam venenatis vehicula odio, et mollis leo laoreet ac. Sed aliquet imperdiet elit, aliquet mattis dui bibendum nec. Pellentesque adipiscing massa nec risus iaculis egestas.

Fusce posuere pretium mi, ac sagittis est posuere quis. Donec blandit, diam eget tempus condimentum, libero lorem placerat erat, id sagittis felis elit non enim. Integer rutrum metus sit amet nulla elementum sed accumsan nunc hendrerit. Morbi ac libero sed felis ultricies lacinia et ac tortor. Vestibulum blandit semper massa non consequat. In volutpat, odio in eleifend consequat, eros ante ultrices nibh, quis faucibus ipsum neque eu diam. Cras dignissim, ipsum ac laoreet elementum, metus sapien cursus orci, quis pretium nibh arcu id erat. Duis imperdiet auctor lectus, ac tempus neque sodales luctus. Sed pharetra, turpis ac placerat aliquet, lectus elit interdum mi, quis ultrices elit nisi ut tortor. Nam tempor feugiat arcu, adipiscing gravida quam iaculis id. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc ac ullamcorper justo. Sed vel neque sed turpis luctus accumsan id sed felis. In ultricies commodo velit, sit amet ultricies lacus tempor vel. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Mauris luctus pellentesque aliquet.

Suspendisse dignissim dictum est at faucibus. Nulla quis nunc id lacus porta pulvinar. Ut accumsan massa at eros euismod eleifend blandit et mauris. Nulla non justo lorem, euismod semper lectus. Fusce dolor elit, auctor in dignissim sed, lacinia eget tortor. Fusce dignissim nulla et turpis adipiscing tempor. Suspendisse at nibh libero. Sed commodo, ipsum eget venenatis tempus, nibh est interdum felis, rhoncus venenatis sapien quam non mi. Donec pulvinar felis porttitor lorem fringilla id porta odio lobortis. Suspendisse hendrerit volutpat cursus. Sed et magna sit amet ligula facilisis posuere. Duis pharetra tristique mi, ut sagittis urna adipiscing in. Maecenas mauris nisl, porta ac pulvinar id, luctus at neque. In dolor enim, semper vel pellentesque non, ultrices nec diam. Donec in sem ultricies quam consectetur ultricies.

Quisque lorem mi, condimentum et iaculis vel, adipiscing id arcu. Vivamus magna elit, dignissim sit amet porttitor a, feugiat nec ligula. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sem neque, suscipit in porttitor vestibulum, accumsan pretium mauris. Ut ac ornare augue. Aenean sodales euismod aliquet. Nam sed orci nisl, nec faucibus dui. Aliquam erat volutpat. Suspendisse in viverra arcu.

Suspendisse enim purus, ornare quis faucibus sodales, pretium vitae nulla. Integer molestie hendrerit mi at tincidunt. Nam non magna leo, quis congue diam. Duis et elementum ipsum. Donec gravida pretium risus ac tempor. Nulla a diam eget tortor varius cursus. Praesent dictum lacinia congue. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse in metus sit amet nisl placerat lobortis. Etiam ac lectus nibh. Donec sagittis, purus in fermentum lobortis, lectus orci consectetur sem, vitae aliquam nunc libero vitae metus. Mauris ut nibh tellus.

Cras eu felis at ipsum vulputate accumsan. Nunc in nibh vel felis bibendum vestibulum. Morbi sit amet augue lacus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus non eros eu purus fringilla ultrices. Sed diam nisl, varius eu cursus et, vulputate in est. Nunc interdum mi id dolor elementum ullamcorper. Nunc rutrum laoreet purus vel sodales. Cras justo nisl, iaculis eget ultricies id, gravida ut mi. Mauris sodales semper fringilla. Vivamus nunc tortor, rhoncus sit amet dignissim vitae, ultrices quis est. Mauris eu lectus eget nisl gravida facilisis. Aenean fermentum fringilla tellus vel venenatis. Duis pretium porttitor ipsum, sed aliquam lacus commodo eget. Nullam suscipit iaculis dolor, quis aliquam leo molestie eu. Duis eget velit eu ligula fringilla tempor.

Nunc elementum viverra ligula. Sed fringilla enim at libero commodo ac lacinia leo fermentum. Nullam sed nisi orci, id feugiat lorem. Phasellus interdum bibendum nisl et gravida. Vestibulum a massa libero, eget consequat sapien. Nullam nunc massa, ultrices et vulputate non, ullamcorper hendrerit sapien. Phasellus id tellus nibh. Etiam fermentum ornare venenatis. Sed facilisis risus at velit blandit posuere. Nam malesuada, erat at lacinia adipiscing, magna tellus suscipit libero, at dictum mi odio in enim. Etiam eu mollis nisl. Vivamus tincidunt consequat facilisis. Curabitur vitae tortor ut tortor placerat sodales sit amet quis erat. Phasellus in libero quis odio lacinia elementum non vel augue. Phasellus felis metus, convallis eu feugiat ultrices, venenatis sodales sem. Nulla et sem sit amet est feugiat cursus.

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam venenatis blandit felis, nec luctus nunc placerat non. Donec condimentum bibendum dolor, ut aliquet lacus mollis eu. Sed id orci augue, at egestas nulla. Sed dui mi, consectetur sit amet ullamcorper nec, ornare et sem. Mauris in aliquet erat. Nunc at mi id lectus vulputate hendrerit. Donec eget blandit justo. Curabitur tempor, mi sed placerat iaculis, arcu nibh fringilla lorem, ut scelerisque augue purus nec turpis. Proin dapibus risus nec risus fringilla non facilisis ligula semper. Cras gravida sem id leo gravida egestas. Donec vel urna quis justo suscipit ultricies. Curabitur mi est, scelerisque sed ullamcorper vel, accumsan quis dolor. Curabitur ut urna metus, a blandit odio.

Morbi et massa pulvinar nunc rhoncus consequat sit amet vitae tortor. Nunc vitae nisl nec odio vulputate accumsan. Quisque ac nulla lorem, eu aliquam tellus. Vivamus at massa odio, non accumsan tortor. Curabitur mauris elit, pellentesque eget pharetra in, placerat rutrum arcu. Nulla condimentum ullamcorper ornare. Etiam eget mi purus. Curabitur et lorem felis. Duis in feugiat leo. Fusce elementum metus venenatis eros molestie aliquet. Duis lobortis accumsan vestibulum.

Morbi velit dolor, sagittis a condimentum in, facilisis et nibh. Donec ac ullamcorper lorem. Aliquam bibendum nulla libero, placerat molestie turpis. Nullam nec arcu vitae sem volutpat accumsan. Nam rhoncus lacus nec lacus viverra eget sagittis purus adipiscing. Nunc mattis metus varius mi suscipit at blandit nunc lobortis. Aliquam nec dolor tortor, sit amet pulvinar tellus.

Duis vestibulum nisi et ligula eleifend laoreet. Sed eget augue massa. Integer in erat egestas nulla aliquam mattis. Sed mollis, lorem eu facilisis vulputate, velit arcu convallis arcu, eu convallis quam nibh non odio. In condimentum metus eu magna fermentum eu congue velit ultricies. Integer sed lorem nec urna aliquet ultrices ac vitae velit. Suspendisse vitae odio felis. Etiam vel consectetur odio. Cras vel sem sed elit gravida ornare vel ut felis. Etiam vel justo in ante aliquet facilisis dignissim a turpis. Suspendisse fringilla leo quis orci tristique tristique. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam mauris lacus, tincidunt commodo lacinia et, vulputate rutrum dui.

Nam tincidunt orci non nulla egestas gravida. Nunc nibh felis, tempor non interdum non, tristique id purus. Aliquam ligula nunc, dignissim ac ornare ac, egestas sed tellus. Morbi neque orci, venenatis ac feugiat pretium, pharetra ac elit. Etiam interdum semper felis. Sed in leo tortor, ut gravida nunc. Cras augue metus, cursus sed eleifend ut, elementum eu tellus. Pellentesque pellentesque egestas ante, in posuere erat congue sed. Vivamus aliquam, lorem vitae congue facilisis, felis augue accumsan odio, at adipiscing nisi ligula in est. Integer quis tempor dolor. Vivamus purus odio, laoreet et consequat non, elementum vitae mauris. Donec molestie posuere diam, eu aliquam orci semper sed. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vestibulum fermentum dui at purus porta et imperdiet odio accumsan. Integer aliquam sapien vel massa scelerisque sagittis.

Aenean egestas lorem at orci tincidunt quis aliquam massa tempus. Curabitur tristique est sed lorem tincidunt at placerat ligula dignissim. Etiam leo arcu, varius sed blandit nec, dapibus vel justo. In convallis lorem non lectus volutpat nec blandit nibh vestibulum. Etiam ipsum orci, semper vitae aliquet at, iaculis non est. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla facilisi.

Fusce tincidunt tristique turpis vel cursus. Duis nec porta justo. Morbi ac justo sed leo volutpat accumsan eu sed urna. Nunc venenatis sodales rutrum. Mauris porttitor justo id urna posuere porttitor. Cras eu nunc nec eros porttitor sodales. Donec varius commodo quam ac ornare.

Aliquam dignissim felis neque, vitae faucibus velit. Suspendisse libero tellus, scelerisque sed dictum et, interdum at nisl. Donec rutrum imperdiet orci, non eleifend nisl commodo vel. Nullam blandit, libero a condimentum bibendum, sem sem aliquam velit, at vestibulum lorem neque vitae tortor. Duis eu neque tortor, at semper risus. Donec vestibulum, magna vel molestie imperdiet, purus lectus ultrices ligula, ac consequat risus ligula vitae erat. Morbi volutpat ullamcorper orci. Pellentesque vulputate auctor lacinia. Fusce lacus est, rutrum sit amet consequat fringilla, ultrices id elit. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin sit amet ornare metus. Phasellus elit erat, posuere quis eleifend at, bibendum sed arcu.

Praesent a turpis sit amet lacus placerat tincidunt quis at neque. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Maecenas cursus purus a urna fringilla id porttitor augue egestas. Aenean sit amet libero turpis, commodo congue libero. Nulla eget ornare eros. Fusce accumsan arcu vel lectus eleifend lacinia. Aenean tristique porta nisi id pellentesque. Proin mattis, lorem eu consequat ornare, tellus ante tristique orci, eget aliquet magna orci a ante. Mauris eget nisi erat, sed consequat lorem. Suspendisse vitae lectus dui. Nulla tincidunt ante sed diam laoreet mattis. Vestibulum ut facilisis neque. Etiam ut odio sit amet nisi ultrices ultrices eu eget nunc. Mauris ut molestie odio. Vivamus luctus, nisl vitae ornare sagittis, lectus risus tempus arcu, nec luctus ligula orci non lacus.

Proin ut nisl quam, eget elementum quam. Suspendisse a condimentum leo. Cras risus tellus, lobortis sit amet mollis id, venenatis quis dolor. Integer pulvinar mollis diam at fermentum. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Duis adipiscing, sapien ac lacinia aliquam, arcu sapien vulputate urna, non rutrum nunc est non tellus. Proin at arcu non eros semper eleifend sit amet id dui. Suspendisse potenti. Fusce convallis risus nisi. Pellentesque eget orci non sapien pulvinar vehicula eget eget magna.

Nunc ac pretium elit. Donec eget diam in augue scelerisque ornare ac ut augue. Nulla volutpat, urna ac commodo dignissim, felis justo auctor neque, non vehicula nisl elit at libero. Suspendisse interdum nulla at orci tincidunt porttitor. Pellentesque accumsan malesuada egestas. Etiam facilisis elit sed enim hendrerit pellentesque sagittis sit amet turpis. Ut libero nisi, euismod molestie iaculis eu, commodo quis ligula.

Sed dictum dictum massa, vitae sagittis tellus dignissim quis. Donec tincidunt, nibh sit amet porta pharetra, nisl felis hendrerit enim, vitae varius erat quam id massa. Nam bibendum ultricies ligula, sed accumsan tellus laoreet id. Quisque id dui bibendum nulla sagittis fringilla ac et nibh. Sed ante dolor, convallis id egestas ac, ullamcorper non ante. Vestibulum dignissim nisi at nibh tempor vestibulum vitae et enim. Vestibulum ipsum ante, facilisis eu condimentum eu, pretium vel orci. Aenean rhoncus urna ac lorem tincidunt mollis in a enim. Vivamus a nulla felis. Nulla elementum, turpis porta sollicitudin condimentum, enim purus faucibus quam, a aliquam quam odio tristique massa. Sed orci turpis, placerat at scelerisque et, sollicitudin in tortor. In ultrices ullamcorper magna, et suscipit eros vulputate nec. Aenean consectetur nibh ut urna venenatis ullamcorper. Aenean egestas posuere arcu, eget convallis odio consequat ac. Ut sem felis, tempor non congue sit amet, tempor sed nunc.

Suspendisse turpis mi, pellentesque vitae vehicula ut, rutrum eget tortor. Vivamus id dolor a justo consequat aliquet. Phasellus vitae laoreet lectus. Nunc commodo, nulla nec pharetra ullamcorper, nulla odio rhoncus felis, vestibulum viverra lorem elit a erat. Fusce augue eros, adipiscing auctor sodales facilisis, tincidunt quis ante. Mauris dapibus convallis diam, a venenatis enim rhoncus non. Ut id tellus est, ac porttitor turpis. Nunc magna lorem, faucibus eget dictum eu, imperdiet nec purus. Nulla facilisis nisi vel nibh gravida a dignissim turpis lacinia. Quisque laoreet, massa at pellentesque placerat, metus neque volutpat augue, et commodo ipsum tellus sed massa. Nunc mi quam, molestie nec pretium quis, mollis eget nulla. Phasellus fermentum mi vel justo fermentum eu viverra mauris sollicitudin. Ut egestas elementum arcu. Suspendisse gravida, magna eu venenatis dapibus, felis massa dictum arcu, eu facilisis massa nisl ut sapien. Etiam a nisi nunc. Nulla fringilla sapien vitae tellus tincidunt sodales.

Nam dapibus nibh ut tellus condimentum posuere. Duis aliquam mauris velit. Integer a euismod risus. Maecenas accumsan augue consequat turpis ullamcorper auctor. Nulla fringilla dapibus nisi a consectetur. Morbi quam sapien, imperdiet pharetra mattis blandit, hendrerit nec velit. Praesent scelerisque semper est, sit amet porttitor lorem vehicula vestibulum. Phasellus condimentum, justo ac semper interdum, metus lorem scelerisque nisi, ac tempus nisi augue at nisi. In mattis faucibus enim, ut tristique orci gravida in. Sed vitae magna sit amet risus tincidunt sollicitudin. Maecenas nec nibh eget magna fringilla dictum ac quis mi. Fusce leo eros, gravida vel accumsan mattis, suscipit eu nunc.

Pellentesque eget elit eget metus condimentum convallis. Nunc varius nibh eu est tincidunt hendrerit. Etiam et elit sit amet est malesuada vehicula. Aenean iaculis, tortor id lobortis gravida, mauris tellus egestas neque, quis lobortis nibh quam nec mi. Curabitur eget augue eget arcu rhoncus facilisis sed sed elit. Aliquam varius tempor est, in cursus lacus interdum vel. Etiam vel dui sit amet lacus tristique aliquam. Quisque condimentum tincidunt lacinia. Etiam bibendum venenatis tortor, sed ornare lorem hendrerit sit amet. Quisque risus ligula, scelerisque non euismod gravida, tincidunt a erat. Integer quis velit in nulla placerat auctor et vel enim. Cras at risus sit amet diam euismod posuere porta ac mauris. Cras a felis purus, sed luctus erat. Morbi gravida orci sem, ut placerat nibh. Mauris commodo elit et leo mollis vel tristique leo sollicitudin.

Curabitur dolor massa, tempus vitae gravida nec, scelerisque in odio. Morbi scelerisque elit sit amet lectus bibendum aliquet. Nullam quis augue vitae nulla tristique tristique. Praesent malesuada orci et lectus auctor quis egestas augue aliquam. Vestibulum eget quam justo. Curabitur pharetra tincidunt lacus, quis molestie dolor elementum ac. Maecenas sit amet augue vel libero tincidunt viverra sit amet viverra metus. Praesent aliquet porta tortor, non posuere ligula semper et. Proin eu ante ligula, in euismod magna. Quisque dictum enim nec velit pharetra placerat. Etiam adipiscing nisl sit amet orci porta ultrices non nec dolor.

Vestibulum vel magna non tortor ullamcorper hendrerit sit amet vitae odio. In vehicula, turpis id porttitor suscipit, arcu nisl aliquet mi, sit amet blandit lorem tellus id neque. In ultricies adipiscing interdum. Quisque sollicitudin, elit feugiat pulvinar placerat, mi elit lacinia dui, sed dictum quam enim quis ipsum. Cras lacinia laoreet interdum. Nulla vehicula ipsum elit. Nam dapibus tincidunt purus. Ut vestibulum egestas odio et suscipit.

Pellentesque aliquet gravida sollicitudin. Aliquam laoreet, diam ac malesuada pretium, libero odio blandit neque, consequat tristique dui erat in tellus. Nunc consectetur sollicitudin congue. Etiam molestie nunc eu tellus venenatis venenatis. Nunc nibh augue, semper non malesuada eget, ornare aliquet sapien. Morbi pretium lorem in libero fermentum vitae laoreet justo faucibus. Integer sollicitudin lacus a nulla bibendum convallis. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vivamus suscipit ultricies elementum. Nam in sodales nibh.

In bibendum convallis tempus. Sed et tellus ac tellus pharetra ultrices. Etiam laoreet enim non turpis tincidunt scelerisque semper turpis faucibus. Nam viverra suscipit mi. Suspendisse tincidunt iaculis neque eu sollicitudin. Donec dictum tellus eget justo rutrum vel auctor est placerat. Pellentesque et neque orci, vel feugiat massa. Nulla sit amet arcu non eros egestas tincidunt. Morbi tempor bibendum auctor. Integer nec tempus sem. Curabitur sed arcu massa.

Cras vestibulum ligula eget nisl consectetur molestie. Phasellus at elit ligula, vel suscipit ligula. Donec posuere orci in elit semper quis ultrices sem aliquam. Praesent tempor nunc id augue ultricies volutpat tincidunt lorem tincidunt. Nunc condimentum consequat dui sed consectetur. Sed pellentesque pulvinar quam non hendrerit. Donec a nulla massa, a imperdiet velit. Etiam ac ipsum quis ipsum fermentum scelerisque in at odio. Proin ut urna orci, sit amet interdum justo. Curabitur felis massa, rhoncus vel congue vel, tincidunt lobortis quam. Aliquam laoreet enim vel elit molestie ut dapibus enim bibendum. Sed eget quam tempus nibh varius fermentum. Maecenas vel sem eu leo molestie ornare. Pellentesque pharetra accumsan venenatis. Vivamus auctor commodo dapibus.

Mauris sem felis, lobortis et elementum vel, eleifend elementum enim. Sed vel sapien a odio dictum lacinia ut quis diam. Aenean viverra turpis non justo tempus bibendum. Morbi varius, eros non egestas volutpat, lectus ante volutpat est, non dignissim nibh lectus posuere turpis. Etiam suscipit nisl nec orci euismod gravida. Nam vel tellus orci, id adipiscing turpis. Vivamus nec diam magna. Donec pharetra diam at neque varius eget vulputate mauris interdum. In consectetur lorem a elit faucibus sit amet facilisis libero pellentesque. Aenean ac sapien nunc, nec accumsan velit. Donec felis orci, interdum et interdum in, eleifend gravida tortor.

Fusce aliquam quam ut turpis venenatis at mollis ante adipiscing. Nulla facilisi. Vivamus congue urna sagittis nunc imperdiet venenatis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla sapien nisl, consectetur et elementum id, consequat ac turpis. Nulla ac augue sit amet nulla sollicitudin sollicitudin nec ac eros. Nam faucibus arcu ut urna lacinia euismod et et urna. Ut scelerisque, lacus id sagittis varius, sem velit ultricies lorem, quis ultrices elit turpis in dui. Pellentesque dapibus iaculis dapibus. Donec posuere, erat nec venenatis molestie, massa massa commodo turpis, eget lacinia eros lacus et tellus.

Integer facilisis turpis non libero dignissim in iaculis lacus vestibulum. Vestibulum nec diam vitae nulla congue consequat ut eget libero. Nulla mattis nibh hendrerit arcu pulvinar pulvinar. Pellentesque facilisis magna iaculis augue pharetra viverra. Ut mollis pharetra nibh vel laoreet. Vestibulum ut arcu diam. Sed gravida hendrerit erat ac sodales.

Integer elementum, elit at commodo tristique, purus dolor sollicitudin risus, nec imperdiet ligula est non risus. In hac habitasse platea dictumst. Praesent id metus nisi, accumsan laoreet tellus. Donec elementum libero quis nisl suscipit ut adipiscing ligula dapibus. Duis pellentesque, tellus eget porta fermentum, eros quam bibendum sem, et facilisis velit lorem sed mi. Pellentesque eu sem sed urna pretium dignissim rutrum ut ante. Ut eget nibh erat. Cras congue commodo eros vitae fringilla. Donec velit ipsum, ornare vitae lobortis ac, bibendum at lectus. Quisque tincidunt molestie commodo. Nullam vulputate mauris nec sapien aliquam accumsan. Nunc et leo vel mi imperdiet accumsan et ut sapien.

Vivamus mollis quam mollis tortor pharetra sit amet faucibus eros posuere. Ut aliquam egestas sem, eget lobortis elit eleifend nec. Sed rutrum malesuada eros, nec sollicitudin sapien auctor quis. Ut euismod, justo vitae vehicula eleifend, erat augue aliquam libero, id aliquam tortor odio non risus. Praesent euismod, elit sit amet rhoncus accumsan, lorem quam scelerisque lorem, non fermentum nunc augue at nibh. Nunc fringilla, elit id imperdiet rutrum, quam mi ullamcorper velit, ut pulvinar nisi massa a leo. Vestibulum tincidunt fermentum metus, a laoreet eros consectetur id. Nulla facilisi. Nunc pharetra viverra odio fringilla hendrerit. Fusce fringilla pharetra urna et aliquam. Curabitur ac ornare orci. Suspendisse potenti.

Nulla at nunc quis dolor tincidunt pellentesque nec sed lorem. Fusce quam erat, pulvinar id molestie vitae, mollis et est. Fusce dolor neque, malesuada a malesuada quis, dapibus eu enim. Quisque viverra eleifend condimentum. Pellentesque cursus venenatis mattis. Etiam bibendum ante id mi dapibus sed consequat sapien lacinia. Ut quam libero, ultricies eu viverra vel, vehicula eget turpis. Nunc congue nisi et lacus ultrices fermentum venenatis eros consectetur. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.

Suspendisse vitae nulla eget odio tincidunt sodales. Sed nec sapien elit, tincidunt tristique augue. Donec in purus ipsum, at vulputate libero. Integer convallis risus vel sapien sodales sodales. Maecenas libero dolor, pretium et consequat vel, ornare vitae lorem. Mauris sit amet felis magna, non dapibus justo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Praesent sed fringilla nisi. In consectetur ornare tellus, eget tincidunt odio placerat quis.

Nullam mollis porta eleifend. Mauris semper venenatis blandit. Nam non velit eu nisi tempor condimentum sed eget libero. Cras rhoncus luctus elit at tempor. Vivamus non magna quam, vel convallis nulla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Ut non pretium lacus.

Donec eu interdum nisi. Suspendisse tincidunt purus vel sapien iaculis auctor. Praesent orci dolor, sodales in volutpat nec, egestas nec nisi. Praesent faucibus, velit dapibus suscipit laoreet, erat erat bibendum nunc, ut semper sapien magna vitae tortor. Integer vitae ipsum et lacus dignissim egestas nec nec odio. Donec pulvinar porttitor lacus quis fermentum. Phasellus id tortor ante, ut condimentum nisi. Pellentesque at purus laoreet erat luctus facilisis sed quis odio. Quisque magna elit, imperdiet a feugiat at, ornare eget urna. Praesent pellentesque nibh at purus iaculis ultricies. Pellentesque vulputate pretium pharetra. Sed imperdiet gravida elementum.

Vivamus interdum varius metus, eu auctor augue mollis facilisis. Aliquam sed dolor ante. Donec vitae rutrum ante. Vestibulum commodo laoreet turpis, in eleifend mi tempor a. Curabitur arcu tellus, feugiat ac aliquam in, eleifend at arcu. Nullam dolor nibh, accumsan quis scelerisque et, aliquet ac lorem. Suspendisse libero risus, ullamcorper sed semper in, imperdiet et neque. Suspendisse potenti. Curabitur ac dui elit. Proin ac turpis dolor. Suspendisse nec nunc at metus venenatis faucibus id non sem. Sed egestas, erat non molestie consequat, neque ligula hendrerit sem, quis viverra tortor elit non elit.

Suspendisse ullamcorper ligula at lacus sodales tincidunt. Aliquam tempus, lorem nec congue commodo, sapien massa dignissim massa, id lobortis nulla ante lacinia ante. Quisque ac lorem quis elit varius pellentesque vestibulum vitae dolor. Aliquam blandit blandit venenatis. Vestibulum vitae sapien ipsum. Praesent id lacus vitae augue rutrum suscipit eu in augue. Phasellus lacinia, massa nec convallis pulvinar, nisi massa bibendum lectus, non mattis sem mauris ac nisi. Nam id sapien vel dolor elementum iaculis. Donec porttitor est et mi lobortis eget laoreet mi bibendum.

Sed hendrerit, magna sed cursus faucibus, ipsum leo tempus mi, et accumsan risus massa vitae dolor. Nunc interdum risus et nisi iaculis rhoncus. Vivamus tincidunt tortor vel nisi venenatis pharetra at nec libero. Ut porttitor lacus sed diam eleifend at interdum ligula dictum. Maecenas ultricies tortor et augue dignissim molestie. Maecenas tincidunt nisi convallis ante ultrices eu condimentum augue semper. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Curabitur porttitor, nisl nec commodo euismod, neque urna convallis lorem, nec ullamcorper ipsum libero sed elit. Ut sagittis ligula sed turpis egestas eget elementum massa accumsan. Proin sagittis interdum lectus, in commodo ante pulvinar non.

Nulla vitae neque neque. Etiam fermentum, leo ac suscipit lobortis, dolor urna porta arcu, quis egestas nisi orci eget elit. Morbi id nisl fermentum mi sollicitudin lacinia. Morbi ac leo nisl, nec aliquet mi. Donec ac enim orci, sit amet varius leo. Nullam neque magna, lacinia quis euismod eget, pellentesque eget nibh. Duis vulputate tortor nec nisl feugiat luctus. Vivamus ultricies nisi libero, sed rhoncus quam. Sed quis nisi urna, quis aliquam tortor. Ut pellentesque tempus erat, et tempus nibh facilisis sit amet. Suspendisse vel ipsum lobortis sem facilisis fermentum sit amet nec magna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis consectetur orci nec ipsum ornare venenatis.

Nunc eleifend lacinia feugiat. Sed diam nulla, luctus a placerat eget, commodo eu ligula. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin augue risus, congue vel semper bibendum, mattis ultrices purus. Phasellus enim risus, pharetra non ornare nec, fringilla nec erat. Vestibulum at felis fringilla lacus malesuada consequat non id tortor. Suspendisse iaculis, leo id faucibus tempus, lectus quam eleifend est, nec lobortis risus turpis vitae diam. Donec ac lacus massa.

Pellentesque mattis leo vitae risus scelerisque luctus. Sed ipsum dolor, malesuada placerat fringilla id, tincidunt vitae arcu. Aliquam at eros nulla, at consectetur nisl. Donec sed nibh nec elit euismod placerat sed nec dui. Duis id lorem quam, in sagittis augue. Donec eu semper est. Cras viverra diam eget orci fringilla a pulvinar augue sollicitudin. Cras nisl diam, dictum quis placerat eget, varius ullamcorper mauris. Cras convallis dui id velit pulvinar condimentum.

Maecenas vestibulum ligula at lectus dapibus vitae pharetra mauris porta. Cras aliquet eleifend lectus, sit amet ornare dui venenatis sed. Curabitur eleifend, libero non mollis sagittis, ligula sem eleifend urna, eget aliquam erat leo id velit. Praesent id augue urna. Maecenas fringilla dignissim suscipit. Nulla iaculis odio justo, in aliquet risus. Sed eget lacinia ante. Nulla sodales, metus fringilla cursus pharetra, leo risus pretium purus, nec faucibus ligula massa ut felis. Morbi eleifend sagittis consectetur. Mauris iaculis rhoncus massa, volutpat tristique orci sollicitudin sagittis. Etiam rhoncus, felis posuere cursus convallis, velit quam dignissim quam, non imperdiet turpis lorem bibendum dui. Donec in convallis mi. In adipiscing leo sit amet turpis ornare ultrices at ut leo. Aenean pulvinar bibendum justo, et tristique lorem hendrerit non.

Sed scelerisque augue sed turpis ultrices tristique. Duis metus purus, mollis id ultrices non, cursus egestas elit. Mauris eget neque quis dolor imperdiet ornare a ac nisl. Aenean ac sem a nisl pulvinar congue. Phasellus mauris augue, volutpat vel porttitor a, placerat vel mauris. Maecenas nisi tellus, facilisis in interdum quis, malesuada consectetur tellus. In et porttitor erat. Aliquam erat volutpat.

Donec mattis mattis aliquam. In sagittis vehicula mauris vitae imperdiet. Integer iaculis metus sit amet enim tincidunt congue. Etiam dolor augue, feugiat id mattis vitae, sagittis non nulla. Fusce semper sem sit amet massa cursus ornare sit amet eget mi. Pellentesque nec nulla eget velit rhoncus pulvinar a eget lacus. Mauris hendrerit feugiat auctor. Fusce nisl ligula, porta bibendum fermentum condimentum, pharetra in arcu. Nulla non ante nisi. Nulla lacus massa, interdum at congue ut, ultrices non turpis. Cras id pretium libero. In at dolor nisl. Donec sit amet elit velit, ac vestibulum lectus.

Nam nunc erat, suscipit vitae vehicula eu, condimentum sed dolor. Mauris laoreet, sapien nec facilisis egestas, risus turpis tincidunt turpis, sed convallis sapien felis id eros. In eget nulla sit amet elit pulvinar egestas nec consectetur neque. Quisque at iaculis enim. Etiam viverra tristique est, at faucibus sapien ornare ut. Donec vitae suscipit mauris. Maecenas a tortor sit amet nisi tempus porta tincidunt ac lorem. Duis dolor eros, ultrices ut fringilla quis, eleifend id risus. Curabitur ut dolor in odio eleifend porta eu ultrices diam. Aenean non risus lacus, et ornare nisl. Maecenas et odio sed odio vulputate sodales. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Aenean a arcu nec felis rutrum elementum.";
			#endregion
	}
}
