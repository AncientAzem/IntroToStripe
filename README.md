# IntroToStripe

In order to build this project, you will need to add the following environment variables to the `launchSettings.json`:
```
"STRIPE_PK": "pk_test_<your-publishable-key>",
"STRIPE_SK": "sk_test_<your-secret-key>"
```

To run the project and auto-rebuild on file changes, run the following command:
```
dotnet watch run
```

While I am giving this talk live, the demo can be accessed at http://stripe.demo.azem.xyz/. Should the presentation be over and you want to run this locally to a public URL, you can create a tunnel using the following command:
```
ssh -R 80:localhost:5000 localhost.run
```

To generate the presentation from markdown:
```
npm install -g @marp-team/marp-cli
marp public/part2.md -o public/index.html
```
