module Tests

open System
open PuppyDataMapper
open PuppyDataMapper.Tool
open Xunit

[<Fact>]
let ``My test`` () =
    let result = JsonToSql.GenerateCode (Mapper())
    Assert.True(true)