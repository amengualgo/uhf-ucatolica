//
//  QRcodeView.m
//  RFID_ios
//
//  Created by hjl on 2018/10/19.
//  Copyright © 2018年  . All rights reserved.
//

#import "QRcodeView.h"
#import <Masonry.h>
#import "QRcodeViewCell.h"
#import "QRCodeScanTypeSelectBtn.h"

//iPhone_X layout

#define SCREEN_WIDTH              [UIScreen mainScreen].bounds.size.width

#define SCREEN_HEIGHT             [UIScreen mainScreen].bounds.size.height
#define iPhone_X                 (SCREEN_HEIGHT == 812.0)
#define Status_H                 (iPhone_X ? 44 : 20)
#define NavBar_H                  44
#define Nav_Height                (Status_H + NavBar_H)
#define Tab_Height                (iPhone_X ? 83 : 49)
#define ScaleW(value)             (value/375.0 * SCREEN_WIDTH)
#define iPhoneX_Bottom_Margin      20
#define Bottom_Margin              (iPhone_X ? -20 : 0)
#define BannerH                    ScaleW(186.0f)
#define weakSelf(self) __weak typeof(self)weakSelf = self

@implementation QRcodeView

-(instancetype)initWithFrame:(CGRect)frame
{
    if (self=[super initWithFrame:frame]) {
        
        self.barcodeParsingType = BarcodeParsingOfASCII;
        
        self.tableView=[[UITableView alloc]initWithFrame:CGRectZero style:UITableViewStylePlain];
        [self addSubview:self.tableView];
        
        self.tableView.delegate=self;
        self.tableView.dataSource=self;
        [self.tableView mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.top.equalTo(self);
            make.bottom.equalTo(self).offset(-130);
        }];
        self.tableView.separatorStyle = UITableViewCellSeparatorStyleNone;
        self.tableView.tableFooterView=[UIView new];
        [self.tableView registerClass:[QRcodeViewCell class] forCellReuseIdentifier:@"aaa"];
        
        self.typeSelectedBtn = [[UIButton alloc]init];
        [self.typeSelectedBtn setTitle:@"default" forState:UIControlStateNormal];
        [self.typeSelectedBtn setTitle:@"default" forState:UIControlStateHighlighted];
        [self.typeSelectedBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.typeSelectedBtn setTitleColor:[UIColor blackColor] forState:UIControlStateHighlighted];
        self.typeSelectedBtn.backgroundColor = [UIColor clearColor];
        [self.typeSelectedBtn addTarget:self action:@selector(clickTypeSelectBtnAction:) forControlEvents:UIControlEventTouchUpInside];
        [self addSubview:self.typeSelectedBtn];
        self.typeSelectedBtn.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        [self.typeSelectedBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.mas_offset(10);
            make.right.mas_offset(-10);
            make.height.mas_equalTo(45);
            make.bottom.mas_equalTo(-100);
        }];
        
        CGFloat width=([UIScreen mainScreen].bounds.size.width-30)/2.0;
        self.scanBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.scanBtn];
        [self.scanBtn setTitle:@"Single Scan" forState:UIControlStateNormal];
        [self.scanBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.scanBtn.backgroundColor=[UIColor colorWithRed:210/255.0 green:210/255.0 blue:210/255.0 alpha:210/255.0];
        [self.scanBtn addTarget:self action:@selector(scanbtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.scanBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.bottom.equalTo(self).offset(-3);
            make.bottom.mas_equalTo(-50);
            make.height.mas_equalTo(40);
            make.width.mas_equalTo(width);
        }];

        self.cleanBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.cleanBtn];
        self.cleanBtn.backgroundColor=[UIColor colorWithRed:210/255.0 green:210/255.0 blue:210/255.0 alpha:210/255.0];
        [self.cleanBtn setTitle:@"Clear" forState:UIControlStateNormal];
        [self.cleanBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.cleanBtn addTarget:self action:@selector(clearbtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.cleanBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.right.equalTo(self).offset(-10);
            make.bottom.equalTo(self).offset(-3);
            make.bottom.mas_equalTo(-50);
            make.height.mas_equalTo(40);
            make.width.mas_equalTo(width);
        }];
        
        
        self.continuousScanBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.continuousScanBtn];
        [self.continuousScanBtn setTitle:@"Continuous Scan" forState:UIControlStateNormal];
        [self.continuousScanBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        self.continuousScanBtn.backgroundColor=[UIColor colorWithRed:210/255.0 green:210/255.0 blue:210/255.0 alpha:210/255.0];
        [self.continuousScanBtn addTarget:self action:@selector(continuousScanbtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.continuousScanBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(10);
            make.bottom.equalTo(self).offset(-3);
//            make.bottom.mas_equalTo(Bottom_Margin);
            make.height.mas_equalTo(40);
            make.width.mas_equalTo(width);
        }];

        self.continuousStopBtn=[UIButton buttonWithType:UIButtonTypeCustom];
        [self addSubview:self.continuousStopBtn];
        self.continuousStopBtn.backgroundColor=[UIColor colorWithRed:210/255.0 green:210/255.0 blue:210/255.0 alpha:210/255.0];
        [self.continuousStopBtn setTitle:@"Continuous Stop" forState:UIControlStateNormal];
        [self.continuousStopBtn setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
        [self.continuousStopBtn addTarget:self action:@selector(continuousStopbtnn) forControlEvents:UIControlEventTouchUpInside];
        [self.continuousStopBtn mas_makeConstraints:^(MASConstraintMaker *make) {
            make.right.equalTo(self).offset(-10);
            make.bottom.equalTo(self).offset(-3);
//            make.bottom.mas_equalTo(Bottom_Margin);
            make.height.mas_equalTo(40);
            make.width.mas_equalTo(width);
        }];
        
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
    NSMutableString *text = [[NSMutableString alloc]init];
    if (self.barcodeParsingType == BarcodeParsingOfASCII) {
        text = [[NSMutableString alloc] initWithData:self.dataSource[indexPath.row] encoding:NSASCIIStringEncoding];
    } else if (self.barcodeParsingType == BarcodeParsingOfUTF8) {
        text = [[NSMutableString alloc]initWithData:self.dataSource[indexPath.row] encoding:NSUTF8StringEncoding];
    } else if (self.barcodeParsingType == BarcodeParsingOfGB2312) {
        NSStringEncoding enc = CFStringConvertEncodingToNSStringEncoding(kCFStringEncodingGB_2312_80);
        text = [[NSMutableString alloc]initWithData:self.dataSource[indexPath.row] encoding:enc];
    }
    
//    if ([RFIDBlutoothManager shareManager].isBarcodeCodeID) {
//        NSMutableString *temp = [[NSMutableString alloc]init];
//        if ([text hasPrefix:@"P"]) {
//            NSString *codeId = [text substringToIndex:3];
//            [temp appendString:[AppHelper getBarcodeTypeByCodeID:codeId]];
//            [temp appendString:[text substringFromIndex:3]];
//            
//        } else {
//            NSString *codeId = [text substringToIndex:1];
//            [temp appendString:[AppHelper getBarcodeTypeByCodeID:codeId]];
//            [temp appendString:[text substringFromIndex:1]];
//        }
//        text = [NSMutableString stringWithString:temp];
//    }
      
    CGFloat labelH = [text boundingRectWithSize:CGSizeMake([UIScreen mainScreen].bounds.size.width - 30, CGFLOAT_MAX) options:NSStringDrawingUsesLineFragmentOrigin | NSStringDrawingTruncatesLastVisibleLine attributes:nil context:nil].size.height;
    return labelH + 30;
}

-(UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    QRcodeViewCell *cell = [tableView dequeueReusableCellWithIdentifier:@"aaa" forIndexPath:indexPath];
    if (self.barcodeParsingType == BarcodeParsingOfASCII) {
        cell.titleLab.text = [[NSString alloc]initWithData:self.dataSource[indexPath.row] encoding:NSASCIIStringEncoding];
    } else if (self.barcodeParsingType == BarcodeParsingOfUTF8) {
        cell.titleLab.text = [[NSString alloc]initWithData:self.dataSource[indexPath.row] encoding:NSUTF8StringEncoding];
    } else if (self.barcodeParsingType == BarcodeParsingOfGB2312) {
         NSStringEncoding enc = CFStringConvertEncodingToNSStringEncoding(kCFStringEncodingGB_2312_80);
        cell.titleLab.text = [[NSString alloc]initWithData:self.dataSource[indexPath.row] encoding:enc];
    }
    
//    NSString *text = cell.titleLab.text;
//    if ([RFIDBlutoothManager shareManager].isBarcodeCodeID) {
//        NSMutableString *temp = [[NSMutableString alloc]init];
//        if ([text hasPrefix:@"P"]) {
//            NSString *codeId = [text substringToIndex:3];
//            [temp appendString:[AppHelper getBarcodeTypeByCodeID:codeId]];
//            [temp appendString:[text substringFromIndex:3]];
//            
//        } else {
//            NSString *codeId = [text substringToIndex:1];
//            [temp appendString:[AppHelper getBarcodeTypeByCodeID:codeId]];
//            [temp appendString:[text substringFromIndex:1]];
//        }
//        cell.titleLab.text = temp;
//    }
    
    CGFloat labelH = [cell.titleLab.text boundingRectWithSize:CGSizeMake([UIScreen mainScreen].bounds.size.width - 30, CGFLOAT_MAX) options:NSStringDrawingUsesLineFragmentOrigin | NSStringDrawingTruncatesLastVisibleLine attributes:nil context:nil].size.height;
    [cell.titleLab mas_updateConstraints:^(MASConstraintMaker *make) {
        make.height.mas_equalTo(labelH + 30);
    }];
    return cell;
}


-(void)scanbtnn
{
    if (self.scanBlock) {
        self.scanBlock();
    }
}
-(void)clearbtnn
{
    if (self.cleanBlock) {
        self.cleanBlock();
    }
}
-(void)continuousScanbtnn
{
    if (self.continuousScanBlock) {
        self.continuousScanBlock();
    }
}
-(void)continuousStopbtnn
{
    if (self.continuousStopBlock) {
        self.continuousStopBlock();
    }
}

-(void)clickTypeSelectBtnAction:(UIButton *)btn {
    QRCodeScanTypeSelectBtn *typeBtn = [[QRCodeScanTypeSelectBtn alloc]init];
    weakSelf(self);
    [typeBtn setClickDefaultBtnBlock:^(UIButton * _Nonnull btn) {
        weakSelf.barcodeParsingType = BarcodeParsingOfASCII;
        [weakSelf.typeSelectedBtn setTitle:btn.currentTitle forState:UIControlStateNormal];
        [weakSelf.typeSelectedBtn setTitle:btn.currentTitle forState:UIControlStateHighlighted];
        [[RFIDBlutoothManager shareManager] setBarcodeParsingType: BarcodeParsingOfASCII];
    }];
    [typeBtn setClickUtf8BtnBlock:^(UIButton * _Nonnull btn) {
        weakSelf.barcodeParsingType = BarcodeParsingOfUTF8;
        [weakSelf.typeSelectedBtn setTitle:btn.currentTitle forState:UIControlStateNormal];
        [weakSelf.typeSelectedBtn setTitle:btn.currentTitle forState:UIControlStateHighlighted];
        [[RFIDBlutoothManager shareManager] setBarcodeParsingType: BarcodeParsingOfUTF8];
    }];
    [typeBtn setClickGb2312BtnBlock:^(UIButton * _Nonnull btn) {
        weakSelf.barcodeParsingType = BarcodeParsingOfGB2312;
        [weakSelf.typeSelectedBtn setTitle:btn.currentTitle forState:UIControlStateNormal];
        [weakSelf.typeSelectedBtn setTitle:btn.currentTitle forState:UIControlStateHighlighted];
        [[RFIDBlutoothManager shareManager] setBarcodeParsingType: BarcodeParsingOfGB2312];
    }];
    
    [[UIApplication sharedApplication].keyWindow addSubview:typeBtn];
}

@end
