你是否在初学 .net core时，被依赖注入所折磨？

你是否在开发过程中，为了注入依赖而不停的在Startup中增加注入代码，而感到麻烦？

你是否考虑过或寻找过能轻松实现自动注入的组件？

如果有，那请欢迎继续往下看。

或许你是被我这标题给吸引过来的，请不要怀疑自己的眼睛，如果你真的遇到过以上的问题，那我相信我的分享能帮助到你。

再次声明，我不是标题党。

闲话少说，此组件已经在我们公司内部使用半年有余，虽然代码不多，但也确确实实为公司同事解决些许麻烦事。为了响应公司开源的号召，所以决定将此组件开源。

在没有此工具之前，相信大多数使用core的程序员都是使用如下方式注入依赖的：


```
services.AddTransient<IStudentRepository, StudentRepository>();
services.AddTransient<IGroupRepository, GroupRepository>();
services.AddTransient<ISchoolRepository, SchoolRepository>();
.....
.....
.....
.....
此处省略若干行
```

但在项目的开发过程中，需要依赖注入的类存在频繁变动的情况，而时常会出现写了实体以及对应的接口后，而忘记添加注入代码情况。此情况不但增加了开发人员的重复而无意义的工作量，也可能会导致出现一些无谓的bug。

从上面的代码中我们可以发现，每一行的代码都非常相似，都是把一个接口和实现类注入到系统中，并设置生命周期为Transient，关于依赖注入相关的知识如果有不了解的，可以参考博客园中其他大神的讲解，这不是我这篇文章的重点。

那既然代码非常相似，那么我们就应该可以想办法封装一下，让代码去实现自动注入，至此，AutoDI组件应运而生。

使用AutoDI组件的方法如下：

首先，定义一个接口，示例如下：


```
    public interface IStudentServices : IScopedAutoDIable
    {
        int GetId();
    }
```

其次，定义接口的实现类：


```
    public class StudentServices : IStudentServices
    {
        public int GetId()
        {
            return 1;
        }
    }
```
细心的朋友应该已经发现了，我上面代码的IStudentServices接口继承了IScopedAutoDIable，从字面意思应该就能看出这个接口是做什么。

> 注：接口和实现类的命名规则需要满足接口为Ixxx，类名为xxx的格式。

什么？看不懂英文？那好吧，我解释下吧，使用了AutoDI的项目在启动的时候，或自动搜索程序集中继承了IScopedAutoDIable的接口或类，找到后，会调用TryAddScoped方法将接口或类注入到系统中。同理，如果想将注入的生命周期改为瞬时的或者单例的，那只需要分别继承ITransientAutoDIable和ISingletonAutoDIable类即可。

最后，也是最重要的一步，在Startup类ConfigureServices方法中，写入如下代码：

```
services.AutoDI();
```

以上代码将会遍历程序所引用的所有程序集中继承了IAutoDIable接口的接口或类，然后实现自动注入。

当然了，有比较较真的或对代码有洁癖的人，可能会说，我不想遍历所有的程序集，这样会影响程序的启动速度。那咱也是有办法满足的，你只需在调用AutoDI接口时，将需要遍历的程序集指定即可。示例代码如下：

```
 services.AutoDI(typeof(IAutoDIEntity).Assembly);
```
上述的代码中，程序将在IAutoDIEntity接口所在的类库中遍历继承了IAutoDIable接口的接口或类。这里写的IAutoDIEntity只是辅助获取程序集的接口，并无实际意义，你也可以换成你想遍历的程序集的任意的类或者接口。

到这里，或许还是会有人说，这还不能满足我的要求，我的接口和类已经继承了IXXXX接口了，我不想继承你定义的IAutoDIable这几个相关的接口，我想继承我自定义的接口。我只能说。。。。

可以，你想干嘛都可以。

如下所示：

```
services.AutoDI<IXXXX>();
```

到这里，此组件的使用方式已经描述完毕，有不清楚的小伙伴可以想办法与我联系。具体怎么联系，随你。反正我也不会搭理你们的。O(∩_∩)O哈哈~

> 不过，如果你在github上给我点了star的话，我或许可以考虑翻下你的牌子。


最后留下源码git地址：
```
https://github.com/billsking/WeShare.AutoDI
```

nuget请搜索：WeShare.AutoDI


---

再最后，发个武汉 .net 召集令

目前各地.net俱乐部活动做了一期又一期，我们大武汉竟然一场活动都没做过，笔者非常着急，非常想为社区贡献一份力量，所以想筹备2020年武汉.net俱乐部活动，目前已经准备了一些分享资料，预计2020年3月或者4月举办第一期线下活动。如果您有场地支持，有好的经验分享或者一些好的想法，请一定要留言告诉我，期待我们武汉的第一次活动能顺利举行。

点击链接加入群聊【武汉.Net微软技术俱乐部】：https://jq.qq.com/?_wv=1027&k=5YbyGhL