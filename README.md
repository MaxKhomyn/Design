# Design

Extended c# library that include classes and styles to create design for WPF application.

### Preparation

Add XAML namespace.

Add ResourceDictionary to App.xaml.

```xml
<Application.Resources>
  <ResourceDictionary Source="pack://application:,,,/Design;component/Styles/Dictionary.xaml"/>
</Application.Resources>
```
### Include

* Localization - to create multilanguage application.
* Themes
* Styles:
  * Window
  * Scroll
  * TextBox
  * TextBlock
  * ListBox
  * TreeView
  * Button
