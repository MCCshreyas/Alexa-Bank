#include<iostream>
using namespace std;
struct node
{
    int data;
    node *next;
};
class sll
{
    public:
    node *head,*tail,*temp;
    void create();
    void insert();
    void display();
    sll()
    {
        head=tail=NULL;
    
    }
};
void sll::create()
{
    node *nnode
    nnode=new node;
    head=nnode;
    nnode->next=NULL;
}
void sll insert()
{
head=temp;
if(temp->next==NULL)
{
temp->next=nnode;
}
else
temp=temp->next;

}
void sll::display()
{
    while(temp!=NULL)
    cout<<temp->data;
    temp->next;
}
int main()
{
    sll s1;
    s1.create()
    s1.insert()
    s1.display()
    return 0;

}