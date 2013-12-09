using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;


public class Index_Auto_2fQuizzes_2fByHost : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fQuizzes_2fByHost()
	{
		this.ViewText = @"from doc in docs.Quizzes
select new { Host = doc.Host }";
		this.ForEntityNames.Add("Quizzes");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "Quizzes", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Host = doc.Host,
				__document_id = doc.__document_id
			});
		this.AddField("Host");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Host");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Host");
		this.AddQueryParameterForReduce("__document_id");
	}
}
