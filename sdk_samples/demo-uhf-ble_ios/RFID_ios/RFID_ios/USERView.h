//
//  USERView.h
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>

@interface USERView : UIView

@property (nonatomic,strong)UILabel *writeLab;
@property (nonatomic,strong)UILabel *readLab;

@property (nonatomic,strong)UITextField *writeField;
@property (nonatomic,strong)UITextField *readField;
@property (nonatomic,strong)UITextField *addressField;
@property (nonatomic,strong)UITextField *lengthField;

@property (nonatomic,copy)void (^readBlock)(void);
@property (nonatomic,copy)void (^writeBlock)(void);


@end
