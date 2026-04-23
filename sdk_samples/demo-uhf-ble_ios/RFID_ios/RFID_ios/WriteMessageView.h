//
//  WriteMessageView.h
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>

@interface WriteMessageView : UIView


@property (nonatomic,strong)UILabel *topLab;
@property (nonatomic,strong)UILabel *addressLab;
@property (nonatomic,strong)UILabel *lengthLab;
@property (nonatomic,strong)UILabel *dataLab;

@property (nonatomic,strong)UILabel *saveLab;
@property (nonatomic,strong)UILabel *addressLab1;
@property (nonatomic,strong)UILabel *lengthLab1;
@property (nonatomic,strong)UILabel *passLab;
@property (nonatomic,strong)UILabel *dataLab1;
@property (nonatomic,strong)UIButton *writeMessageBtn;


@property (nonatomic,strong)UIButton *enableBtn;
@property (nonatomic,strong)UITextField *addressField;
@property (nonatomic,strong)UITextField *lengthField;
@property (nonatomic,strong)UITextField *dataField;

@property (nonatomic,strong)UIButton *epcBtn;
@property (nonatomic,strong)UIButton *tidBtn;
@property (nonatomic,strong)UIButton *userBtn;

@property (nonatomic,strong)UIButton *saveBtn;

@property (nonatomic,strong)UITextField *addressField1;
@property (nonatomic,strong)UITextField *lengthField1;
@property (nonatomic,strong)UITextField *dataField1;
@property (nonatomic,strong)UITextField *passWordField1;

@property (nonatomic,assign)BOOL isFilter;

@property (nonatomic,copy)NSString *typeStr;

@property (nonatomic,copy)void (^saveBlock)(void);
@property (nonatomic,copy)void (^writeMessageBlock)(void);

@end
