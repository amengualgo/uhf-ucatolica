//
//  QRcodeViewCell.m
//  RFID_ios
//
//  Created by hjl on 2018/10/30.
//  Copyright © 2018年  . All rights reserved.
//

#import "QRcodeViewCell.h"
#import <Masonry.h>
@implementation QRcodeViewCell

-(instancetype)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier
{
    if (self=[super initWithStyle:style reuseIdentifier:reuseIdentifier]) {
      
        self.titleLab=[UILabel new];
        [self addSubview:self.titleLab];
        self.titleLab.textColor=[UIColor colorWithRed:111/255.0 green:111/255.0 blue:111/255.0 alpha:1];
        self.titleLab.numberOfLines = 0;
        self.titleLab.font=[UIFont systemFontOfSize:15];
        [self.titleLab mas_makeConstraints:^(MASConstraintMaker *make) {
            make.centerY.equalTo(self);
            
            make.left.equalTo(self).offset(15);
            make.right.equalTo(self).offset(-20);
        }];
        
    }
    return self;
}

- (void)awakeFromNib {
    [super awakeFromNib];
    // Initialization code
}

- (void)setSelected:(BOOL)selected animated:(BOOL)animated {
    [super setSelected:selected animated:animated];

    // Configure the view for the selected state
}

@end
