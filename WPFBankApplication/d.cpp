#include<iostream>
using namespace std;
struct node
{
    int data;
    node *next;
};
class sll
{
    node *head,*tail
public:
    sll()
    {
        head=tail=NULL;
    }
void insert();
//void delet();
void display();
}
void sll::insert()
{
cout<<"enter the data "<<endl;
cin>>m;
node *nnode=new node;
nnode->data=m;
nnode->next=NULL;
}
void sll::display()
{
    do
    {

        cout<<"entered data is "<<nnode->data;
    }
    
}
int main()
{
    sll s1;
    s1.insert();
    s1.display();
}





