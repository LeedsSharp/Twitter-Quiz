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


public class Index_Auto_2fUsers_2fByUsername : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fUsers_2fByUsername()
	{
		this.ViewText = @"from doc in docs.Users
select new { Username = doc.Username }";
		this.ForEntityNames.Add("Users");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "Users", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Username = doc.Username,
				__document_id = doc.__document_id
			});
		this.AddField("Username");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Username");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Username");
		this.AddQueryParameterForReduce("__document_id");
	}
}
