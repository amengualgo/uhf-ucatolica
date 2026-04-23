//
//  ScanViewCell.m
//  RFID_ios
//
//  Created by   on 2018/5/10.
//  Copyright © 2018年  . All rights reserved.
//

#import "ScanViewCell.h"
#import <Masonry.h>

#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]
#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
@implementation ScanViewCell

- (void)awakeFromNib {
    [super awakeFromNib];
    // Initialization code
}

-(instancetype)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier
{
    if (self=[super initWithStyle:style reuseIdentifier:reuseIdentifier]) {
        
        self.epcLab=[UILabel new];
        [self addSubview:self.epcLab];
        self.epcLab.textColor=RGB(111, 111, 111, 1);
        self.epcLab.font=[UIFont systemFontOfSize:14];
        self.epcLab.lineBreakMode = NSLineBreakByWordWrapping;
        self.epcLab.numberOfLines=0;
        //self.epcLab.text=@"1336546465saffdfrgrgrthththth thtrh h htrghtgtg tg";
        [self.epcLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(5);
            make.centerY.equalTo(self);
            make.height.mas_equalTo(300); //行最大高度，最小高度由外层TableView约束
            make.right.equalTo(self).offset(-AdaptW(98));
        }];
        
        _countLab=[UILabel new];
        [self addSubview:_countLab];
        _countLab.textColor=RGB(111, 111, 111, 1);
        _countLab.font=[UIFont systemFontOfSize:16];
        //_countLab.text=@"12";
        _countLab.numberOfLines=0;
        [_countLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self->_epcLab);
            make.width.mas_equalTo(40);
            make.right.equalTo(self).offset(-AdaptW(54));
        }];
        
        _rssiLab=[UILabel new];
        [self addSubview:_rssiLab];
        _rssiLab.textColor=RGB(111, 111, 111, 1);
        _rssiLab.font=[UIFont systemFontOfSize:16];
        //_rssiLab.text=@"N/A";
        _rssiLab.textAlignment = NSTextAlignmentCenter;
        [_rssiLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.top.bottom.equalTo(self->_epcLab);
            make.width.mas_equalTo(70);
            make.right.equalTo(self).offset(-AdaptW(2));
        }];
    }
    return self;
}

- (void)setSelected:(BOOL)selected animated:(BOOL)animated {
    [super setSelected:selected animated:animated];

    // Configure the view for the selected state
}

@end
