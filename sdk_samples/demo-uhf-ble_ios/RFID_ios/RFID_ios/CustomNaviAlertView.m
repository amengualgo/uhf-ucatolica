//
//  CustomNaviAlertView.m
//  RFID_ios
//
//  Created by   on 2018/4/26
//  Copyright © 2018年  . All rights reserved.
//

#import "CustomNaviAlertView.h"
#import "ShadeCell.h"
#import <Masonry.h>
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]

@interface CustomNaviAlertView ()<UITableViewDataSource, UITableViewDelegate>
{
 
}
@end

@implementation CustomNaviAlertView


- (instancetype)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        [self setUI];
        
    }
    return self;
}
- (void)setUI{
    self.backgroundColor=[[UIColor blackColor] colorWithAlphaComponent:0.5];
    
    UIButton *removeBtn=[UIButton buttonWithType:UIButtonTypeRoundedRect];
    [self addSubview:removeBtn];
    [removeBtn addTarget:self action:@selector(remove) forControlEvents:UIControlEventTouchUpInside];
    [removeBtn mas_makeConstraints:^(MASConstraintMaker *make) {
        make.edges.equalTo(self);
    }];
    
    UIView *bigView=[UIView new];
    [self addSubview:bigView];
    bigView.backgroundColor=[UIColor blackColor];
    bigView.layer.borderWidth=1.5;
    bigView.clipsToBounds=YES;
    bigView.layer.borderColor=RGB(150, 150, 150, 1).CGColor;
    [bigView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.center.equalTo(self);
        make.height.mas_equalTo(420);
        make.width.mas_equalTo(320);
        
    }];
    UILabel *topLab=[UILabel new];
    [bigView addSubview:topLab];
   // topLab.text=@"BtDemo";
    topLab.font=[UIFont boldSystemFontOfSize:19];
    topLab.textColor=RGB(236, 236, 236, 1);
    [topLab mas_makeConstraints:^(MASConstraintMaker *make) {
        make.top.equalTo(bigView).offset(10);
        make.left.equalTo(bigView).offset(10);
        make.height.mas_equalTo(25);
        make.width.mas_equalTo(150);
    }];
    
    UILabel *seleLab=[UILabel new];
    [bigView addSubview:seleLab];
    seleLab.textColor=RGB(236, 236, 236, 1);
    seleLab.text=@"Select a device";
    seleLab.textAlignment=NSTextAlignmentCenter;
    seleLab.backgroundColor=RGB(110, 110, 110, 1);
    [seleLab mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.equalTo(bigView).offset(3);
        make.right.equalTo(bigView).offset(-3);
        make.top.equalTo(bigView).offset(50);
        make.height.mas_equalTo(35);
    }];
    
    self.cannelBtn = [UIButton buttonWithType:UIButtonTypeCustom];
    [bigView addSubview:self.cannelBtn];
    self.cannelBtn.backgroundColor = RGB(220, 220, 220, 1);
    self.cannelBtn.layer.cornerRadius = 1;
    [self.cannelBtn setTitle:@"Cancel" forState:UIControlStateNormal];
    [self.cannelBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [self.cannelBtn addTarget:self action:@selector(cancel) forControlEvents:UIControlEventTouchUpInside];
    [self.cannelBtn mas_makeConstraints:^(MASConstraintMaker *make) {
        make.bottom.equalTo(bigView).offset(-8);
        make.left.equalTo(bigView).offset(6);
        make.right.equalTo(bigView).offset(-6);
        make.height.mas_equalTo(42);
    }];
    
    self.myTableView = [[UITableView alloc]initWithFrame:CGRectZero style:UITableViewStylePlain];
    self.myTableView.delegate = self;
    self.myTableView.dataSource = self;
    [self.myTableView setSeparatorColor:RGB(31, 31, 31, 1)];
    self.myTableView.backgroundColor=[UIColor blackColor];
    [self.myTableView registerClass:[ShadeCell class] forCellReuseIdentifier:@"aaa"];
    [bigView addSubview:self.myTableView];
    [self.myTableView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.right.equalTo(bigView);
        make.top.equalTo(seleLab.mas_bottom);
        make.bottom.equalTo(self.cannelBtn.mas_top);
    }];
    
    
    
    
    
}

-(void)remove
{
    if (self.removeBlock) {
        self.removeBlock();
    }
}
- (void)cancel
{
    
    if (self.cancelBlock) {
        self.cancelBlock();
    }
    
    
}
- (NSMutableArray *)dataSource{
    
    if (_dataSource == nil) {
        _dataSource = [[NSMutableArray alloc] init];
    }
    return _dataSource;
}

#pragma mark ---------tableView代理方法
- (NSInteger) tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section{
    if (self.dataSource.count != 0) {
        return self.dataSource.count;
    }
    return 0;
}
-(CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    return 51;
}
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath{
    
    ShadeCell *cell =[tableView dequeueReusableCellWithIdentifier:@"aaa" forIndexPath:indexPath];
    cell.backgroundColor=[UIColor blackColor];
    if (self.dataSource.count != 0) {
        BLEModel *model = self.dataSource[indexPath.row];
        cell.nameLab.text=model.nameStr;
        cell.identyLab.text=model.addressStr;
        cell.rssiLab.text=[NSString stringWithFormat:@"Rssi=%@",model.rssStr];
        
        return cell;
    }
    return cell;
}
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    if (self.dataSource.count != 0) {
        BLEModel *model = self.dataSource[indexPath.row];
        self.selectItem(model);
        [self removeFromSuperview];
    }
}

- (void)didSelectItem:(didSelectedItem)selectItem{
    
    self.selectItem = selectItem;
}

/*
// Only override drawRect: if you perform custom drawing.
// An empty implementation adversely affects performance during animation.
- (void)drawRect:(CGRect)rect {
    // Drawing code
}
*/

@end
