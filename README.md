# SnapTest C# NUnit Test Generator

Generates SnapTest suites/tests using C# & NUnit.

*** If you are just getting started, use the [C# project harness](https://github.com/snaptest-io/csharpnunit-harness) to get up and running quickly. 

### To develop/contribute to this generator

First, make sure you have the snaptest-cli tool installed by running `npm install -g snaptest-cli`.
Then use snaptest-cli's custom generator flag to utilize your own branch/fork of this generator:

`snaptest -c <absolute or relative path to generators index.js> ...`

Contributions in the forms of PRs and tickets are welcome.

#### Full Example:

`snaptest -c ./index.js -t iHrsRTzgENFUVI1TPAtIFqyd0QElssxy1TA0X9y`

1. The -c flag specifics a "custom" generator, which lets you use a local generator on your filesystem.  This replaces the -r flag which specifies an official generator.  
1. The -t flag specific the access token that you can access via the snaptest extension.
1. No folder or project is specified, so this will generate all the tests in your personal cloud account.   
 
#### More info

1. snaptest-cli reference at [snaptest-cli](https://www.npmjs.com/package/snaptest-cli)
1. Use the SnapTest extension code generator page to explore your options:

![generate test icons](https://res.cloudinary.com/snaptest/image/upload/v1535423298/READMEs/Screen_Shot_2018-08-27_at_2.53.42_PM.png)
![generate test screens](https://res.cloudinary.com/snaptest/image/upload/v1535423299/READMEs/Screen_Shot_2018-08-27_at_2.55.40_PM.png)

 