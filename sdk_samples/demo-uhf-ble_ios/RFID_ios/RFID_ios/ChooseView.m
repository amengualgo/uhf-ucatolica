//
//  ChooseView.m
//  RFID_ios
//
//  Created by   on 2018/4/27.
//  Copyright © 2018年  . All rights reserved.
//

#import "ChooseView.h"
#import <Masonry.h>
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
@implementation ChooseView

-(instancetype)initWithFrame:(CGRect)frame
{
    if (self=[super initWithFrame:frame]) {
        
        self.backgroundColor = [[UIColor blackColor] colorWithAlphaComponent:0.5];
        
        
        
        self.tableView=[[UITableView alloc]initWithFrame:CGRectZero style:UITableViewStylePlain];
        [self addSubview:self.tableView];
        self.tableView.dataSource=self;
        self.tableView.delegate=self;
        [self.tableView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.center.equalTo(self);
            make.width.mas_equalTo(280);
            make.height.mas_equalTo(360);
        }];
        [self.tableView registerClass:[UITableViewCell class] forCellReuseIdentifier:@"aaa"];
        self.tableView.separatorStyle = UITableViewCellSeparatorStyleNone;
    }
    return self;
}

-(NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
 
     return self.dataSource.count;
}

-(CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    return 43;
}
-(UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    UITableViewCell *cell=[tableView dequeueReusableCellWithIdentifier:@"aaa" forIndexPath:indexPath];
    
    cell.textLabel.text=self.dataSource[indexPath.row];
    cell.textLabel.font = [UIFont systemFontOfSize:16];
    return cell;
}
-(void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    [tableView deselectRowAtIndexPath:indexPath animated:YES];
    self.returnBlock(indexPath.row);
}

-(void)refreshTableViewWith:(NSMutableArray *)dataSource
{
    self.dataSource=dataSource;
    [self.tableView reloadData];
}

-(void)touchesBegan:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event
{
    if (self.removeBlock) {
        self.removeBlock();
    }
}

@end
