//使用此接口可新增状态机节点，Execute方法会在血液(Energy)流过时触发。
namespace Mycelia;
 
 public interface IPulseNode
 {
     float Cost { get; }
     IPulseNode?[] Targets { get; }
 
     void Execute(Energy context);
 }