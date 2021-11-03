# IntroToStripe

In order to build this project, you will need to add the following environemnt variables to the `launchSettings.json`:
```
"STRIPE_PK": "pk_test_<your-publishable-key>",
"STRIPE_SK": "sk_test_<your-secret-key>"
```

To run the project and auto-rebuild on file changes, run the following command:
```
dotnet watch run
```

To tunnel the application to the internet for a live demo, run the following:
```
ssh -R 80:localhost:5000 localhost.run
```

To generate the presentation from markdown:
```
npm install -g @marp-team/marp-cli
marp .\Presentation.md -o index.html
```