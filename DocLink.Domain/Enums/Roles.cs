﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocLink.Domain.Enums
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum Roles
	{
		Admin,
		Doctor,
		Patient
	}
}