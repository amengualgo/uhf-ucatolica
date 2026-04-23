//
//  DestroyView.h
//  RFID_ios
//
//  Created by   on 2018/4/26.
//  Copyright © 2018年  . All rights reserved.
//

#import <UIKit/UIKit.h>

@interface KillView : UIView

@property (nonatomic,strong)UILabel *topLab;
@property (nonatomic,strong)UILabel *addressLab;
@property (nonatomic,strong)UILabel *lengthLab;
@property (nonatomic,strong)UILabel *dataLab;
@property (nonatomic,strong)UILabel *passLab;

@property (nonatomic,strong)UIButton *destoryBtn;

@property (nonatomic,strong)UIButton *enableBtn;
@property (nonatomic,strong)UITextField *addressField;
@property (nonatomic,strong)UITextField *lengthField;
@property (nonatomic,strong)UITextField *dataField;

@property (nonatomic,strong)UIButton *epcBtn;
@property (nonatomic,strong)UIButton *tidBtn;
@property (nonatomic,strong)UIButton *userBtn;


@property (nonatomic,strong)UITextField *passWordField;


@property (nonatomic,assign)BOOL isFilter;

@property (nonatomic,copy)NSString *typeStr;

@property (nonatomic,copy)void (^destoryBlock)(void);

@end
