# Contributing to the Dapr State Query Builder project
## Found an issue?
If you find a bug in the source code or a mistake in the documentation, you can help us out by [submitting an issue](https://github.com/innovianhq/Innovian.Contrib.Dapr.StateQueryBuilder/issues/new/choose) to our [GitHub repository](https://github.com/innovianhq/Innovian.Contrib.Dapr.StateQueryBuilder). Even better, 
you can submit a [Pull Request](#submit-pr) with a fix.

## <a name="feature"></a> Want a feature?
You can *request* a new feature by [submitting an issue](https://github.com/innovianhq/Innovian.Contrib.Dapr.StateQueryBuilder/issues/new/choose) to our [GitHub repositpry](https://github.com/innovianhq/Innovian.Contrib.Dapr.StateQueryBuilder). If you would like to *implement* a new feature, please submit an issue with your 
proposed work first before getting started so we can confirm whether we'd accept it or not. We don't want to waste your time, after all. 

* If you would deem the change a **Major Feature**, open an issue and outline your proposal so it can be discussed. This will allow better work coordination, prevent duplication of work and help you craft the change so it'll be successfully accepted into the project.
* For a **Small Feature**, feel free to write it and directly submit it as a [pull request](#submit-pr).

### <a name="submit-pr"></a> Submitting a Pull Request (PR)
Before submitting your Pull Request (PR) consider the following guidelines:
* Search [GitHub](https://github.com/innovianhq/Innovian.Contrib.Dapr.StateQueryBuilder/pulls) for an open or closed PR that relates to your submission to avoid duplicating an existing effort.
* Make your changes in a new git branch:
  ```shell
  git checkout -b my-fix-branch-main
  ```
* Create your patch and add unit tests where possible.
* Commit your changes using a descriptive commit message.
* Push your branch to GitHub:
  ```shell
  git push origin my-fix-branch
  ```
  * In GitHub, send a pull request to `Innovian.Contrib.Dapr.StateQueryBuilder:main`
  * If we suggest changes then:
    * Make the required updates
    * Rebase your branch and force push to your GitHub repository (this will update your Pull Request):
      ```shell
      git rebase main -i
      git push -f
      ```
And that's it! Thank you for your contribution!


Speaking of, I'd like to give a brief thank you to the maintainers of SoCreate's [Service Fabric Distributed Cache](https://github.com/SoCreate/service-fabric-distributed-cache) project for the general layout of the repository documentation.
