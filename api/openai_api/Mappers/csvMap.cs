using System;
using CsvHelper.Configuration;
using openai_api.DTO;

namespace openai_api.Mappers
{
	public sealed class csvMap : ClassMap<csvDTO>
	{
		public csvMap()
		{
			Map(x => x.userEmail).Name("userEmail");
            Map(x => x.clientIP).Name("clientIP");
            Map(x => x.question).Name("question");
            Map(x => x.answer).Name("answer");
            Map(x => x.dlpAns).Name("dlpAns");
            Map(x => x.date).Name("date");
            Map(x => x.type).Name("type");
        }
    }
}

