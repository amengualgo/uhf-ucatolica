//
//  ScanTwoView.m
//  RFID_ios
//
//  Created by   on 2019/6/17.
//  Copyright © 2019年  . All rights reserved.
//

#import "ScanView.h"

#import <Masonry.h>
#import "ScanViewCell.h"
#import "RFIDBlutoothManager.h"
#import "AppHelper.h"

#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue

@interface ScanView ()

@property (nonatomic,strong)UIButton *filterBtn;

@property (nonatomic,strong)UIView *filterView;
@property (nonatomic, strong) MASConstraint *containerHeightConstraint;
@property (nonatomic, strong) MASConstraint *containerTopConstraint;

@property (nonatomic,strong)UIButton *epcBtn;
@property (nonatomic,strong)UIButton *tidBtn;
@property (nonatomic,strong)UIButton *userBtn;

/// 过滤是否显示
@property (nonatomic,assign)BOOL filterVisible;

@end

@implementation ScanView

-(instancetype)initWithFrame:(CGRect)frame
{
    self.filterBank = BANK_EPC;
    self.filterVisible = NO;
    self.dataSource = [NSMutableArray array];
    self.allCount = 0;
    
    if (self=[super initWithFrame:frame]) {
        
        self.filterBtn = [UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.filterBtn];
        [self.filterBtn setImage:[UIImage imageNamed:@"right"] forState:UIControlStateNormal];
        [self.filterBtn addTarget:self action:@selector(filterVisibleBtn) forControlEvents:UIControlEventTouchUpInside];
        [self.filterBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(12);
            make.top.equalTo(self).offset(10);
            make.width.mas_equalTo(20);
            make.height.mas_equalTo(20);
        }];
        UILabel *filterLab=[UILabel new];
        [self addSubview:filterLab];
        filterLab.font=[UIFont systemFontOfSize:18];
        filterLab.text=@"Filter";
        filterLab.textColor=[UIColor blackColor];
        filterLab.userInteractionEnabled = YES;
        UITapGestureRecognizer *tapGesture = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(filterVisibleBtn)];
        [filterLab addGestureRecognizer:tapGesture];
        [filterLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.filterBtn);
            make.left.equalTo(self.filterBtn.mas_right).offset(10);
            make.width.mas_equalTo(40);
            make.height.mas_equalTo(22);
        }];
        
        self.filterView = [UIView new];
        self.filterView.hidden = true;
        [self addSubview:self.filterView];
        self.filterView.layer.masksToBounds=YES;
        self.filterView.layer.borderWidth=1;
        self.filterView .layer.borderColor=RGB(150, 150, 150, 1).CGColor;
        [self.filterView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(5);
            make.right.equalTo(self).offset(-5);
            make.top.equalTo(self.filterBtn.mas_bottom).offset(4);
            //make.height.mas_equalTo(60);
            self.containerTopConstraint = make.top.equalTo(self.filterBtn.mas_bottom).offset(0);
            self.containerHeightConstraint = make.height.mas_equalTo(0);
        }];
        
        UILabel *ptrLab=[UILabel new];
        [self.filterView addSubview:ptrLab];
        ptrLab.text=@"Ptr:";
        ptrLab.font=[UIFont systemFontOfSize:16];
        ptrLab.textColor=RGB(150, 150, 150, 1);
        [ptrLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(self.filterBtn.mas_bottom).offset(10);
            make.height.mas_equalTo(AdaptH(25));
            make.width.mas_equalTo(40);
        }];
        self.ptrField=[UITextField new];
        [self.filterView addSubview:self.ptrField];
        self.ptrField.text = @"32";
        self.ptrField.font=[UIFont systemFontOfSize:16];
        [self.ptrField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(ptrLab.mas_right);
            make.centerY.equalTo(ptrLab);
            make.width.mas_equalTo(75);
            make.height.mas_equalTo(30);
        }];
        UIView *ptrLinview=[UIView new];
        [self.filterView addSubview:ptrLinview];
        ptrLinview.backgroundColor=RGB(150, 150, 150, 1);
        [ptrLinview mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.ptrField.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.ptrField);
        }];
        UILabel *label=[UILabel new];
        [self.filterView addSubview:label];
        label.text=@"(bit)";
        label.font=[UIFont systemFontOfSize:16];
        label.textColor=RGB(150, 150, 150, 1);
        [label mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.ptrField);
            make.left.equalTo(self.ptrField.mas_right);
            make.height.mas_equalTo(22);
            make.width.mas_equalTo(40);
        }];
        
        UILabel *lenLab=[UILabel new];
        [self.filterView addSubview:lenLab];
        lenLab.text=@"Len:";
        lenLab.font=[UIFont systemFontOfSize:16];
        lenLab.textColor=RGB(150, 150, 150, 1);
        [lenLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(label.mas_right).offset(20);
            make.top.bottom.equalTo(ptrLab);
            make.width.mas_equalTo(40);
        }];
        self.lenField=[UITextField new];
        [self.filterView addSubview:self.lenField];
        self.lenField.text = @"";
        self.lenField.font=[UIFont systemFontOfSize:16];
        [self.lenField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(lenLab.mas_right);
            make.centerY.equalTo(lenLab);
            make.width.mas_equalTo(75);
            make.height.mas_equalTo(30);
        }];
        UIView *linview1=[UIView new];
        [self.filterView addSubview:linview1];
        linview1.backgroundColor=RGB(150, 150, 150, 1);
        [linview1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.lenField.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.lenField);
        }];
        UILabel *label1=[UILabel new];
        [self.filterView addSubview:label1];
        label1.text=@"(bit)";
        label1.font=[UIFont systemFontOfSize:16];
        label1.textColor=RGB(150, 150, 150, 1);
        [label1 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self.lenField);
            make.left.equalTo(self.lenField.mas_right);
            make.height.mas_equalTo(22);
            make.width.mas_equalTo(40);
        }];
        
        UILabel *dataLab=[UILabel new];
        [self.filterView addSubview:dataLab];
        dataLab.text=@"Data：";
        dataLab.font=[UIFont systemFontOfSize:16];
        dataLab.textColor=RGB(150, 150, 150, 1);
        [dataLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.top.equalTo(ptrLab.mas_bottom).offset(18);
            make.width.mas_equalTo(50);
        }];
        self.dataField=[UITextField new];
        [self.filterView addSubview:self.dataField];
        self.dataField.font=[UIFont systemFontOfSize:16];
        [self.dataField mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(dataLab.mas_right);
            make.centerY.equalTo(dataLab);
            make.right.equalTo(self).offset(-10);
            make.height.mas_equalTo(30);
        }];
        UIView *linview2=[UIView new];
        [self.filterView addSubview:linview2];
        linview2.backgroundColor=RGB(150, 150, 150, 1);
        [linview2 mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.dataField.mas_bottom);
            make.height.mas_equalTo(1);
            make.left.right.equalTo(self.dataField);
        }];
     
        self.epcBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self.filterView addSubview:self.epcBtn];
        [self.epcBtn setTitle:@"EPC" forState:UIControlStateNormal];
        [self.epcBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.epcBtn.layer.borderWidth=2;
        self.epcBtn.layer.borderColor=[UIColor blueColor].CGColor;
        self.epcBtn.layer.masksToBounds=YES;
        self.epcBtn.layer.cornerRadius=10;
        [self.epcBtn addTarget:self action:@selector(epcbtnn) forControlEvents:UIControlEventTouchUpInside];
        CGFloat bankBtnWidth=([UIScreen mainScreen].bounds.size.width-60)/3.0;
        [self.epcBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(15);
            make.top.equalTo(linview2.mas_bottom).offset(10);
            make.width.mas_equalTo(bankBtnWidth);
            make.height.mas_equalTo(30);
        }];
        
        self.tidBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self.filterView addSubview:self.tidBtn];
        [self.tidBtn setTitle:@"TID" forState:UIControlStateNormal];
        [self.tidBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.tidBtn.layer.borderWidth=2;
        [self.tidBtn addTarget:self action:@selector(tidBtnn) forControlEvents:UIControlEventTouchUpInside];
        self.tidBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
        self.tidBtn.layer.masksToBounds=YES;
        self.tidBtn.layer.cornerRadius=10;
        [self.tidBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.epcBtn.mas_right).offset(15);
            make.top.bottom.equalTo(self.epcBtn);
            make.width.mas_equalTo(bankBtnWidth);
        }];
        
        self.userBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self.filterView addSubview:self.userBtn];
        [self.userBtn setTitle:@"USER" forState:UIControlStateNormal];
        [self.userBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.userBtn.layer.borderWidth=2;
        [self.userBtn addTarget:self action:@selector(userBtnn) forControlEvents:UIControlEventTouchUpInside];
        self.userBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
        self.userBtn.layer.masksToBounds=YES;
        self.userBtn.layer.cornerRadius=10;
        [self.userBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.tidBtn.mas_right).offset(15);
            make.top.bottom.equalTo(self.epcBtn);
            make.width.mas_equalTo(bankBtnWidth);
        }];
        
        self.setFilterBtn =[UIButton buttonWithType:UIButtonTypeCustom];
        [self.filterView addSubview:self.setFilterBtn];
        [self.setFilterBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.setFilterBtn setTitle:@"Set Filter" forState:UIControlStateNormal];
        self.setFilterBtn.backgroundColor=RGB(210, 210, 210, 1);
        self.setFilterBtn.titleLabel.font = [UIFont systemFontOfSize:16];
        [self.setFilterBtn addTarget:self action:@selector(setFilterBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.setFilterBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.right.equalTo(self).offset(-10);
            make.top.equalTo(self.epcBtn.mas_bottom).offset(10);
            make.height.mas_equalTo(AdaptH(35));
        }];
        
        
        
        self.singleBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.singleBtn];
        [self.singleBtn setEnabled:NO];
        [self.singleBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
        [self.singleBtn setTitle:@"Single" forState:UIControlStateNormal];
        self.singleBtn.backgroundColor=RGB(210, 210, 210, 1);
        self.singleBtn.titleLabel.font = [UIFont systemFontOfSize:16];
        [self.singleBtn addTarget:self action:@selector(singleBtnn) forControlEvents:UIControlEventTouchUpInside];
        CGFloat width=([UIScreen mainScreen].bounds.size.width-15*5)/4.0;
        [self.singleBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(12);
            make.top.equalTo(self.filterView.mas_bottom).offset(15);
            make.height.mas_equalTo(AdaptH(35));
            make.width.mas_equalTo(width);
        }];
        
        self.beginBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.beginBtn];
        [self.beginBtn setEnabled:NO];
        [self.beginBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
        self.beginBtn.titleLabel.font = [UIFont systemFontOfSize:16];
        [self.beginBtn setTitle:@"Start" forState:UIControlStateNormal];
        self.beginBtn.backgroundColor=RGB(210, 210, 210, 1);
        [self.beginBtn addTarget:self action:@selector(beginBtnn) forControlEvents:UIControlEventTouchUpInside];
        
        [self.beginBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self.singleBtn.mas_right).offset(15);
            make.top.bottom.equalTo(self.singleBtn);
            make.width.mas_equalTo(width);
        }];
        
        self.stopBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.stopBtn];
        [self.stopBtn setEnabled:NO];
        [self.stopBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
        self.stopBtn.titleLabel.font = [UIFont systemFontOfSize:16];
        [self.stopBtn setTitle:@"Stop" forState:UIControlStateNormal];
        self.stopBtn.backgroundColor=RGB(210, 210, 210, 1);
        [self.stopBtn addTarget:self action:@selector(stopBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.stopBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self.beginBtn);
            make.left.equalTo(self.beginBtn.mas_right).offset(15);
            make.width.mas_equalTo(width);
        }];
        
        self.cleanBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.cleanBtn];
        [self.cleanBtn setEnabled:NO];
        [self.cleanBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
        self.cleanBtn.titleLabel.font = [UIFont systemFontOfSize:16];
        [self.cleanBtn setTitle:@"Clear" forState:UIControlStateNormal];
        self.cleanBtn.backgroundColor=RGB(210, 210, 210, 1);
        [self.cleanBtn addTarget:self action:@selector(cleanBtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.cleanBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self.beginBtn);
            make.right.equalTo(self).offset(-15);
            make.width.mas_equalTo(width);
        }];
        
        _tagLab=[UILabel new];
        [self addSubview:_tagLab];
        _tagLab.text=@"EPC";
        _tagLab.textColor=RGB(111, 111, 111, 1);
        _tagLab.font=[UIFont systemFontOfSize:16];
        [_tagLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.equalTo(self.cleanBtn.mas_bottom).offset(10);
            make.left.equalTo(self).offset(5);
            make.width.mas_equalTo(99);
            make.height.mas_equalTo(25);
        }];
        self.countLab=[UILabel new];
        [self addSubview:self.countLab];
        self.countLab.text=@"0";
        self.countLab.font=[UIFont systemFontOfSize:16];
        self.countLab.textColor=RGB(111, 111, 111, 1);
        [self.countLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self->_tagLab);
            make.width.mas_equalTo(50);
            make.left.equalTo(self).offset(AdaptW(100));
        }];
        
        self.allCountLab=[UILabel new];
        [self addSubview:self.allCountLab];
        self.allCountLab.font=[UIFont systemFontOfSize:16];
        self.allCountLab.text=@"0";
        self.allCountLab.textColor=RGB(111, 111, 111, 1);
        [self.allCountLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self->_tagLab);
            make.width.mas_equalTo(50);
            make.left.equalTo(self).offset(AdaptW(160));
        }];
        
        _countLabString=[UILabel new];
        [self addSubview:_countLabString];
        _countLabString.textColor=RGB(111, 111, 111, 1);
        _countLabString.font=[UIFont systemFontOfSize:16];
        _countLabString.text=@"Count";
        [_countLabString mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self->_tagLab);
            make.width.mas_equalTo(70);
            make.right.equalTo(self).offset(-AdaptW(45));
        }];
        
        UILabel *rssiLab=[UILabel new];
        [self addSubview:rssiLab];
        rssiLab.textColor=RGB(111, 111, 111, 1);
        rssiLab.font=[UIFont systemFontOfSize:16];
        rssiLab.text=@"RSSI";
        [rssiLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self->_tagLab);
            make.width.mas_equalTo(40);
            make.right.equalTo(self).offset(-AdaptW(7));
        }];
        
        self.tableView=[[UITableView alloc]initWithFrame:CGRectZero style:UITableViewStylePlain];
        [self addSubview:self.tableView];
        [self.tableView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.bottom.equalTo(self);
            make.top.equalTo(self->_tagLab.mas_bottom);
        }];
        self.tableView.delegate=self;
        self.tableView.dataSource=self;
        self.tableView.tableFooterView=[UIView new];
        [self.tableView registerClass:[ScanViewCell class] forCellReuseIdentifier:@"aaa"];
    }
    return self;
}
-(NSInteger)numberOfSectionsInTableView:(UITableView *)tableView
{
    return 1;
}
-(NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    return self.dataSource.count;
}
-(CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    UHFTagInfo *tag = self.dataSource[indexPath.row];
    NSString *text = tag.text;
    CGFloat  labelH = [text boundingRectWithSize:CGSizeMake([UIScreen mainScreen].bounds.size.width - 103, CGFLOAT_MAX) options:NSStringDrawingUsesLineFragmentOrigin | NSStringDrawingUsesFontLeading attributes:@{NSFontAttributeName: [UIFont systemFontOfSize:14]} context:nil].size.height;
    return labelH + 20;
}

-(UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    ScanViewCell *cell=[tableView dequeueReusableCellWithIdentifier:@"aaa" forIndexPath:indexPath];
    UHFTagInfo *tag = self.dataSource[indexPath.row];
    cell.epcLab.text = tag.text;
    cell.countLab.text = [NSString stringWithFormat:@"%ld", tag.count];
    cell.rssiLab.text = tag.rssi;
    return cell;
}



-(void) connectState {
    [self.singleBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [self.beginBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [self.stopBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
    [self.singleBtn setEnabled:YES];
    [self.beginBtn setEnabled:YES];
    [self.stopBtn setEnabled:NO];
}
-(void) disConnectState {
    [self.singleBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
    [self.beginBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
    [self.stopBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
    [self.cleanBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [self.singleBtn setEnabled:NO];
    [self.beginBtn setEnabled:NO];
    [self.stopBtn setEnabled:NO];
    [self.cleanBtn setEnabled:YES];
}


-(void)filterVisibleBtn
{
    if (self.filterVisible) {
        self.filterVisible = NO;
        [self.filterBtn setImage:[UIImage imageNamed:@"right"] forState:UIControlStateNormal];
        self.filterView.hidden = true;
        self.containerTopConstraint.equalTo(@0);
        self.containerHeightConstraint.equalTo(@0);
    } else {
        self.filterVisible = YES;
        [self.filterBtn setImage:[UIImage imageNamed:@"down"] forState:UIControlStateNormal];
        self.filterView.hidden = false;
        self.containerTopConstraint.equalTo(@165);
        self.containerHeightConstraint.equalTo(@165);
    }
}

-(void)epcbtnn
{
    self.epcBtn.layer.borderColor=[UIColor blueColor].CGColor;
    self.tidBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.userBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    
    self.filterBank = BANK_EPC;
    self.ptrField.text = @"32";
//    self.lengthField.text=@"";
}

-(void)tidBtnn
{
    self.epcBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.tidBtn.layer.borderColor=[UIColor blueColor].CGColor;
    self.userBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;

    self.filterBank = BANK_TID;
    self.ptrField.text = @"0";
//    self.lengthField.text=@"";
}

-(void)userBtnn
{
    self.epcBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.tidBtn.layer.borderColor=RGB(180, 180, 180, 1).CGColor;
    self.userBtn.layer.borderColor=[UIColor blueColor].CGColor;
    
    self.filterBank = BANK_USER;
    self.ptrField.text = @"0";
//    self.lengthField.text=@"0";
}


-(void)setFilterBtnn
{
    NSNumberFormatter *numberFormatter = [[NSNumberFormatter alloc] init];
    NSString *ptrStr = self.ptrField.text;
    NSNumber *ptr = [numberFormatter numberFromString:ptrStr];
    if (ptrStr.length == 0) {
        [BSprogreUtil showMBProgressWith:self andTipString:@"Ptr can't be empty" autoHide:YES];
        return;
    } else if (ptr == nil) {
        [BSprogreUtil showMBProgressWith:self andTipString:@"Ptr must be a number" autoHide:YES];
        return;
    }
    NSString *lenStr = self.lenField.text;
    NSNumber *len = [numberFormatter numberFromString:lenStr];
    if (lenStr.length == 0) {
        [BSprogreUtil showMBProgressWith:self andTipString:@"Len can't be empty" autoHide:YES];
        return;
    } else if (len == nil) {
        [BSprogreUtil showMBProgressWith:self andTipString:@"Len must be a number" autoHide:YES];
        return;
    }
    NSString *dataStr = self.dataField.text;
    if (len.intValue != 0 && dataStr.length == 0) {
        [BSprogreUtil showMBProgressWith:self andTipString:@"Data can't be empty" autoHide:YES];
        return;
    }
    if (len.intValue != 0 && ![AppHelper isHexString:dataStr]) {
        [BSprogreUtil showMBProgressWith:self andTipString:@"Data must be Hexadecimal" autoHide:YES];
        return;
    }
    if (len.intValue != 0 && dataStr.length * 4 < len.integerValue) {
        [BSprogreUtil showMBProgressWith:self andTipString:@"Filter Data does not match Len" autoHide:YES];
        return;
    }
    
    if (self.setFilterBlock) {
        self.setFilterBlock();
    }
}

-(void)singleBtnn
{
    if (self.singleBlock) {
        self.singleBlock();
    }
}

-(void)beginBtnn
{
    NSLog(@"TAG---beginBtnn");
    
    if (self.beginBlock) {
        self.beginBlock();
    }
    [self.singleBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
    [self.beginBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
    [self.stopBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [self.cleanBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
    [self.singleBtn setEnabled:NO];
    [self.beginBtn setEnabled:NO];
    [self.stopBtn setEnabled:YES];
    [self.cleanBtn setEnabled:NO];
}
-(void)stopBtnn
{
    NSLog(@"TAG---stopBtnn");
    if (self.stopBlock) {
        self.stopBlock();
    }
    [self.singleBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [self.beginBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [self.stopBtn setTitleColor:RGB(133, 133, 133, 1) forState:UIControlStateNormal];
    [self.cleanBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
    [self.singleBtn setEnabled:YES];
    [self.beginBtn setEnabled:YES];
    [self.stopBtn setEnabled:NO];
    [self.cleanBtn setEnabled:YES];
}
-(void)cleanBtnn
{
    NSLog(@"TAG---cleanBtnn");
    if (self.cleanBlock) {
        self.cleanBlock();
    }
}



- (NSString *)parseStrWithOriginalStr:(NSString *)epcAndRssiStr {
    NSString *version = [UIDevice currentDevice].systemVersion;
    if (version.floatValue >= 13.0) {
        if ([epcAndRssiStr containsString:@"bytes = 0x"]) {
            NSRange range = [epcAndRssiStr rangeOfString:@"bytes = 0x"];
            epcAndRssiStr = [epcAndRssiStr substringFromIndex:range.location + range.length];
            epcAndRssiStr = [epcAndRssiStr substringToIndex:epcAndRssiStr.length - 1];
         }
    } else {
         NSString *valueStrr=[epcAndRssiStr substringWithRange:NSMakeRange(1, epcAndRssiStr.length-2)];
         NSArray *aa=[valueStrr componentsSeparatedByString:@" "];
         NSMutableString *bb=[[NSMutableString alloc]init];
         for (NSString *str in aa) {
              [bb appendString:str];
         }
        epcAndRssiStr =[NSString stringWithFormat:@"%@",bb];
    }
    return epcAndRssiStr;
}


/*
// Only override drawRect: if you perform custom drawing.
// An empty implementation adversely affects performance during animation.
- (void)drawRect:(CGRect)rect {
    // Drawing code
}
*/

@end
