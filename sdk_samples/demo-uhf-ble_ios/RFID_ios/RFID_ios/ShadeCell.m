//
//  ShadeCell.m
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import "ShadeCell.h"
#import "Masonry.h"
#define RGB(r,g,b,a) [UIColor colorWithRed:(r)/255.0f green:(g)/255.0f blue:(b)/255.0f alpha:(a)]

#define AdaptW(floatValue) floatValue
#define AdaptH(floatValue) floatValue
@implementation ShadeCell

- (void)awakeFromNib {
    [super awakeFromNib];
    // Initialization code
}
-(instancetype)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier
{
    
    if (self=[super initWithStyle:style reuseIdentifier:reuseIdentifier]) {
        //   self.backgroundColor=[UIColor blackColor];
        self.nameLab=[UILabel new];
        [self addSubview:self.nameLab];
        self.nameLab.font=[UIFont systemFontOfSize:AdaptW(16)];
        self.nameLab.text=@"Nkideew";
        self.nameLab.textColor=RGB(236, 236, 236, 1);
        [self.nameLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.equalTo(self).offset(AdaptW(10));
            make.top.equalTo(self).offset(AdaptH(8));
            make.width.mas_equalTo(AdaptW(260));
            make.height.mas_equalTo(AdaptH(20));
        }];
        self.identyLab=[UILabel new];
        [self addSubview:self.identyLab];
        self.identyLab.textColor=RGB(236, 236, 236, 1);
        self.identyLab.text=@"451531354153135";
        self.identyLab.font=[UIFont systemFontOfSize:13];
        [self.identyLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.left.right.equalTo(self.nameLab);
            make.top.equalTo(self.nameLab.mas_bottom);
            make.height.mas_equalTo(AdaptH(15));
        }];
        
        self.rssiLab=[UILabel new];
        [self addSubview:self.rssiLab];
        self.rssiLab.textColor=RGB(236, 236, 236, 1);
        self.rssiLab.text=@"Rssi=-100";
        self.rssiLab.textAlignment=NSTextAlignmentRight;
        self.rssiLab.font=[UIFont systemFontOfSize:15];
        [self.rssiLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self);
            make.right.equalTo(self).offset(-AdaptW(12));
            make.height.mas_equalTo(25);
            make.width.mas_equalTo(120);
        }];
        
    }
    return self;
}
- (void)setSelected:(BOOL)selected animated:(BOOL)animated {
    [super setSelected:selected animated:animated];

    // Configure the view for the selected state
}

@end
