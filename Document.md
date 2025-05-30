## 查找所有带特性的方法&使用反射注册事件
```c#
 CommandHelper.CacheAttributeMethods(typeof(RegisterCommandLine), typeof(NaButtonAttribute));
 CommandHelper.MethodCache[typeof(RegisterCommandLine)].ForEach(method =>
 {
     CommandHelper.AttributeCache[method].ForEach(attribute =>
     {
         string commandName = string.IsNullOrEmpty((attribute as RegisterCommandLine).Name) ? method.DeclaringType.FullName + "." + method.Name : (attribute as RegisterCommandLine).Name;
         commandEvents.Add(new CommandEventArgs(commandName, method));
     });
 });
```

